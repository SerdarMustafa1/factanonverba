using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class PasswordResetLinkSentModel : Volo.Abp.Account.Web.Pages.Account.PasswordResetLinkSentModel
    {
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }


        public override async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await AccountAppService.SendPasswordResetCodeAsync(
                    new SendPasswordResetCodeDto
                    {
                        Email = Email,
                        AppName = "MVC", //TODO: Const!
                        ReturnUrl = ReturnUrl,
                        ReturnUrlHash = ReturnUrlHash
                    }
                );
            }
            catch (UserFriendlyException e)
            {
                Alerts.Danger(GetLocalizeExceptionMessage(e));
                return Page();
            }
            return await base.OnPostAsync();
        }
    }
}
