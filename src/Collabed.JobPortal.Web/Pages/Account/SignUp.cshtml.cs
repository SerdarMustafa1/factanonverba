using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Volo.Abp.Account.Web.Pages.Account.LoginModel;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class SignUpModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string EmailAddress { get; set; }
        [TempData]
        public int AccountType { get; set; }
        public ExternalProviderModel[] ExternalProviders { get; private protected set; }

        public SignUpModel()
        {
            var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
            var externalProviders = new ExternalProviderModel[] { liProvider };
            this.ExternalProviders = externalProviders;
        }

        public IActionResult OnPost()
        {
            if (Request.Form["Source"].Equals("AccountType"))
            {
                if (int.TryParse(Request.Form["AccountType"], out var accountType))
                {
                    AccountType = accountType;
                }

                return Page();
            }

            return RedirectToPage("AccountType");
        }
    }
}
