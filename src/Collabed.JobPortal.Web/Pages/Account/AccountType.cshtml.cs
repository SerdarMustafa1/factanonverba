using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class AccountTypeModel : PageModel
    {
        [BindProperty]
        public string EmailAddress { get; set; }

        public AccountTypeModel()
        {
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            var request = Request;
        }

    }
}
