using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Validation;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class BMTLoginModel : LoginModel
    {
        public new ExternalProviderModel[] ExternalProviders { get; private protected set; }
        public string LinkedInRedirectUri { get; private set; } = "signin-linkedin";

        [BindProperty]
        [Required(ErrorMessage = "Please type your email address")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string UserNameOrEmailAddress { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please type your password")]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; } = true;

        [TempData]
        public int AccountType { get; set; }

        public BMTLoginModel(IAuthenticationSchemeProvider schemeProvider, IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions)
            : base(schemeProvider, accountOptions, identityOptions)
        {
            var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
            var indeedProvider = new ExternalProviderModel() { DisplayName = "Indeed", AuthenticationScheme = "Indeed" };
            var externalProviders = new ExternalProviderModel[] { liProvider, indeedProvider };
            ExternalProviders = externalProviders;
            ReturnUrl = "";
        }

        public override async Task<IActionResult> OnPostAsync(string action)
        {
            ValidateModel();

            await ReplaceEmailToUsernameOfInputIfNeeds();

            await IdentityOptions.SetAsync();

            var result = await SignInManager.PasswordSignInAsync(
                UserNameOrEmailAddress,
                Password,
                RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = UserNameOrEmailAddress
            });

            if (result.RequiresTwoFactor)
            {
                return await TwoFactorLoginResultAsync();
            }

            if (result.IsLockedOut)
            {
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }

            if (result.IsNotAllowed)
            {
                Alerts.Warning(L["LoginIsNotAllowed"]);
                return Page();
            }

            if (!result.Succeeded)
            {
                Alerts.Danger(L["InvalidUserNameOrPassword"]);
                return Page();
            }

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await UserManager.FindByNameAsync(UserNameOrEmailAddress) ??
                       await UserManager.FindByEmailAsync(UserNameOrEmailAddress);

            Debug.Assert(user != null, nameof(user) + " != null");

            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                var userType = (UserType)Enum.Parse(typeof(UserType), user.GetUserType(), true);
                if (userType == UserType.Organisation)
                {
                    ReturnUrl = "~/joblistings";
                }
                else
                {
                    ReturnUrl = "~/jobdashboard";
                }
            }

            return Redirect(ReturnUrl);
        }

        public override async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (int.TryParse(Request.Form["AccountType"], out var accountType))
            {
                AccountType = accountType;
                TempData["AccountType"] = AccountType;
            }

            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash, AccountType });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["scheme"] = provider;

            return await Task.FromResult(Challenge(properties, provider));
        }

        public override async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string remoteError = null)
        {
            //TODO: Did not implemented Identity Server 4 sample for this method (see ExternalLoginCallback in Quickstart of IDS4 sample)
            /* Also did not implement these:
             * - Logout(string logoutId)
             */

            if (remoteError != null)
            {
                Logger.LogWarning($"External login callback error: {remoteError}");
                return RedirectToPage("./Login");
            }

            await IdentityOptions.SetAsync();

            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                Logger.LogWarning("External login info is not available");
                return RedirectToPage("./Login");
            }

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );

            if (!result.Succeeded)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                    Action = "Login" + result
                });
            }

            if (result.IsLockedOut)
            {
                Logger.LogWarning($"External login callback error: user is locked out!");
                throw new UserFriendlyException("Cannot proceed because user is locked out!");
            }

            if (result.IsNotAllowed)
            {
                Logger.LogWarning($"External login callback error: user is not allowed!");
                throw new UserFriendlyException("Cannot proceed because user is not allowed!");
            }

            if (result.Succeeded)
            {
                var emailAddress = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(emailAddress))
                {
                    return RedirectSafely(string.IsNullOrWhiteSpace(returnUrl) ? "/jobdashboard" : returnUrl, returnUrlHash);
                }

                var currentUser = await UserManager.FindByEmailAsync(emailAddress);
                var userType = (UserType)Enum.Parse(typeof(UserType), currentUser.GetUserType(), true);
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    if (userType == UserType.Candidate)
                    {
                        returnUrl = "/jobdashboard";
                    }
                    else
                    {
                        returnUrl = "/joblistings";
                    }
                }
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            //TODO: Handle other cases for result!

            var email = loginInfo.Principal.FindFirstValue(AbpClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToPage("./Register", new
                {
                    IsExternalLogin = true,
                    ExternalLoginAuthSchema = loginInfo.LoginProvider,
                    ReturnUrl = returnUrl
                });
            }

            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = await CreateExternalUserAsync(loginInfo);
            }
            else
            {
                if (await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey) == null)
                {
                    CheckIdentityErrors(await UserManager.AddLoginAsync(user, loginInfo));
                }
            }

            await SignInManager.SignInAsync(user, false);

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = user.Name
            });

            return RedirectSafely(returnUrl, returnUrlHash);
        }

        protected override async Task<Volo.Abp.Identity.IdentityUser> CreateExternalUserAsync(ExternalLoginInfo info)
        {
            await IdentityOptions.SetAsync();

            var emailAddress = info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = new Volo.Abp.Identity.IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);

            CheckIdentityErrors(await UserManager.CreateAsync(user));
            CheckIdentityErrors(await UserManager.SetEmailAsync(user, emailAddress));
            CheckIdentityErrors(await UserManager.AddLoginAsync(user, info));
            CheckIdentityErrors(await UserManager.AddDefaultRolesAsync(user));

            user.Name = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            user.Surname = info.Principal.FindFirstValue(ClaimTypes.Surname);

            var phoneNumber = info.Principal.FindFirstValue(AbpClaimTypes.PhoneNumber);
            if (!phoneNumber.IsNullOrWhiteSpace())
            {
                var phoneNumberConfirmed = string.Equals(info.Principal.FindFirstValue(AbpClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);
                user.SetPhoneNumber(phoneNumber, phoneNumberConfirmed);
            }

            await UserManager.UpdateAsync(user);

            return user;
        }

        protected override async Task ReplaceEmailToUsernameOfInputIfNeeds()
        {
            if (!ValidationHelper.IsValidEmailAddress(UserNameOrEmailAddress))
            {
                return;
            }

            var userByUsername = await UserManager.FindByNameAsync(UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(UserNameOrEmailAddress);
            if (userByEmail == null)
            {
                return;
            }

            UserNameOrEmailAddress = userByEmail.UserName;
        }
    }
}
