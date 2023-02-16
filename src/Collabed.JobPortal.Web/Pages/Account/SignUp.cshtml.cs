using Collabed.JobPortal.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Account;
using static Volo.Abp.Account.Web.Pages.Account.LoginModel;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class SignUpModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        
        public string EmailAddress { get; set; }
        public ExternalProviderModel[] ExternalProviders { get; private protected set; }

        public SignUpModel()
        {
            var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
            var externalProviders = new ExternalProviderModel[] { liProvider };
            this.ExternalProviders = externalProviders;
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("AccountType", new { id = "123" });
        }
    }
}
