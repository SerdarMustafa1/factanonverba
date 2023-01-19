using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class CollabedLoginModel : LoginModel
    {
        public new ExternalProviderModel[] ExternalProviders { get; private protected set; }
        private readonly string LocalAccountsRedirectUri = "/";
        public string LinkedInRedirectUri { get; private set; } = "signin-linkedin";
        public CollabedLoginModel(IAuthenticationSchemeProvider schemeProvider, IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions)
            : base(schemeProvider, accountOptions, identityOptions)
        {
            var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
            var externalProviders = new ExternalProviderModel[] { liProvider };
            this.ExternalProviders = externalProviders;
            this.ReturnUrl = LocalAccountsRedirectUri;
        }
    }
}
