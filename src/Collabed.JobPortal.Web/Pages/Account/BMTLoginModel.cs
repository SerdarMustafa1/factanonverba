using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class BMTLoginModel : LoginModel
    {
        public new ExternalProviderModel[] ExternalProviders { get; private protected set; }
        public string LinkedInRedirectUri { get; private set; } = "signin-linkedin";
        public BMTLoginModel(IAuthenticationSchemeProvider schemeProvider, IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions)
            : base(schemeProvider, accountOptions, identityOptions)
        {
            var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
            var indeedProvider = new ExternalProviderModel() { DisplayName = "Indeed", AuthenticationScheme = "Indeed" };
            var externalProviders = new ExternalProviderModel[] { liProvider, indeedProvider };
            this.ExternalProviders = externalProviders;
        }
    }
}
