using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BuildMyTalentOAuthExtensions
{
    public class BuildMyTalentOAuthIndeedHandler : OAuthHandler<OAuthOptions>
    {
        protected new HttpClient Backchannel => base.Options.Backchannel;

        //
        // Summary:
        //     The handler calls methods on the events which give the application control at
        //     certain points where processing is occurring. If it is not provided a default
        //     instance is supplied which does nothing when the methods are called.
        protected new OAuthEvents Events
        {
            get
            {
                return base.Events;
            }
            set
            {
                base.Events = value;
            }
        }
        public BuildMyTalentOAuthIndeedHandler(IOptionsMonitor<OAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            string requestUri = Options.UserInformationEndpoint;

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                //await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }
            var responseContent = await response.Content.ReadAsStringAsync(Context.RequestAborted);
            var payload = JsonDocument.Parse(responseContent);
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }


        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            IQueryCollection query = base.Request.Query;
            StringValues stringValues = query["state"];
            AuthenticationProperties properties = base.Options.StateDataFormat.Unprotect(stringValues);
            if (properties == null)
            {
                return HandleRequestResult.Fail("The oauth state was missing or invalid.");
            }

            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.", properties);
            }

            StringValues stringValues2 = query["error"];
            if (!StringValues.IsNullOrEmpty(stringValues2))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append((string)stringValues2);
                StringValues stringValues3 = query["error_description"];
                if (!StringValues.IsNullOrEmpty(stringValues3))
                {
                    stringBuilder.Append(";Description=").Append((string)stringValues3);
                }

                StringValues stringValues4 = query["error_uri"];
                if (!StringValues.IsNullOrEmpty(stringValues4))
                {
                    stringBuilder.Append(";Uri=").Append((string)stringValues4);
                }

                return HandleRequestResult.Fail(stringBuilder.ToString(), properties);
            }

            StringValues stringValues5 = query["code"];
            if (StringValues.IsNullOrEmpty(stringValues5))
            {
                return HandleRequestResult.Fail("Code was not found.", properties);
            }

            OAuthTokenResponse oAuthTokenResponse = await ExchangeCodeAsync(stringValues5, BuildRedirectUri(base.Options.CallbackPath)); 
            if (oAuthTokenResponse.Error != null)
            {
                return HandleRequestResult.Fail(oAuthTokenResponse.Error, properties);
            }

            if (string.IsNullOrEmpty(oAuthTokenResponse.AccessToken))
            {
                return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
            }

            ClaimsIdentity identity = new ClaimsIdentity(ClaimsIssuer);
            if (base.Options.SaveTokens)
            {
                List<AuthenticationToken> list = new List<AuthenticationToken>();
                list.Add(new AuthenticationToken
                {
                    Name = "access_token",
                    Value = oAuthTokenResponse.AccessToken
                });
                if (!string.IsNullOrEmpty(oAuthTokenResponse.RefreshToken))
                {
                    list.Add(new AuthenticationToken
                    {
                        Name = "refresh_token",
                        Value = oAuthTokenResponse.RefreshToken
                    });
                }

                if (!string.IsNullOrEmpty(oAuthTokenResponse.TokenType))
                {
                    list.Add(new AuthenticationToken
                    {
                        Name = "token_type",
                        Value = oAuthTokenResponse.TokenType
                    });
                }

                if (!string.IsNullOrEmpty(oAuthTokenResponse.ExpiresIn) && int.TryParse(oAuthTokenResponse.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
                {
                    DateTimeOffset dateTimeOffset = base.Clock.UtcNow + TimeSpan.FromSeconds(result);
                    list.Add(new AuthenticationToken
                    {
                        Name = "expires_at",
                        Value = dateTimeOffset.ToString("o", CultureInfo.InvariantCulture)
                    });
                }

                properties.StoreTokens(list);
            }
            AuthenticationTicket authenticationTicket = await CreateTicketAsync(identity, properties, oAuthTokenResponse);
            if (authenticationTicket != null)
            {
                return HandleRequestResult.Success(authenticationTicket);
            }
            return HandleRequestResult.Fail("Failed to retrieve user information from remote server.", properties);
        }

        protected async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {
                    "client_id",
                    base.Options.ClientId
                },
                { "redirect_uri", redirectUri },
                {
                    "client_secret",
                    base.Options.ClientSecret
                },
                { "code", code },
                { "grant_type", "authorization_code" }
            });
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, base.Options.TokenEndpoint);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Content = content;
            HttpResponseMessage httpResponseMessage = await Backchannel.SendAsync(httpRequestMessage, base.Context.RequestAborted);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return OAuthTokenResponse.Success(JsonDocument.Parse(await httpResponseMessage.Content.ReadAsStringAsync()));
            }

            return OAuthTokenResponse.Failed(new Exception("OAuth token endpoint failure: " + await Display(httpResponseMessage)));
        }

        private static async Task<string> Display(HttpResponseMessage response)
        {
            StringBuilder output = new StringBuilder();
            output.Append(string.Concat("Status: ", response.StatusCode, ";"));
            output.Append("Headers: " + response.Headers.ToString() + ";");
            StringBuilder stringBuilder = output;
            stringBuilder.Append("Body: " + await response.Content.ReadAsStringAsync() + ";");
            return output.ToString();
        }

        protected override bool ValidateCorrelationId(AuthenticationProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            if (!properties.Items.TryGetValue(".xsrf", out var value))
            {
                //base.Logger.CorrelationPropertyNotFound(base.Options.CorrelationCookie.Name);
                return false;
            }

            properties.Items.Remove(".xsrf");//.AspNetCore.Correlation.LinkedIn.y2hgH14F13FDrQajOafOPSMeIHba37GT_WEL9tcdWMk //.AspNetCore.Correlation.y2hgH14F13FDrQajOafOPSMeIHba37GT_WEL9tcdWMk
            string text = base.Options.CorrelationCookie.Name + value;
            string text2 = base.Request.Cookies[text];
            if (string.IsNullOrEmpty(text2))
            {
                //base.Logger.CorrelationCookieNotFound(text);
                return false;
            }

            CookieOptions options = base.Options.CorrelationCookie.Build(base.Context, base.Clock.UtcNow);
            base.Response.Cookies.Delete(text, options);
            if (!string.Equals(text2, "N", StringComparison.Ordinal))
            {
                //base.Logger.UnexpectedCorrelationCookieValue(text, text2);
                return false;
            }

            return true;
        }

    }
}

