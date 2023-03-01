using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
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
        public bool RememberMe { get; set; }
        public BMTLoginModel(IAuthenticationSchemeProvider schemeProvider, IOptions<AbpAccountOptions> accountOptions, IOptions<IdentityOptions> identityOptions)
            : base(schemeProvider, accountOptions, identityOptions)
        {
            var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
            var indeedProvider = new ExternalProviderModel() { DisplayName = "Indeed", AuthenticationScheme = "Indeed" };
            var externalProviders = new ExternalProviderModel[] { liProvider, indeedProvider };
            ExternalProviders = externalProviders;
            ReturnUrl = "../JobDashboard";
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

            return Redirect(ReturnUrl ?? "~/");
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
