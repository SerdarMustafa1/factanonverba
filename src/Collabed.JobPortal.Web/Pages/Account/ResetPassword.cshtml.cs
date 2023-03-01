using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Collabed.JobPortal.Web.Pages.Account
{
    public class ResetPasswordModel : Volo.Abp.Account.Web.Pages.Account.ResetPasswordModel
    {
        public string Email { get; set; }
        public bool ResetDoneFlag { get; set; }
        public override async Task<IActionResult> OnGetAsync()
        {
            Email = await UserManager.GetEmailAsync(await UserManager.GetByIdAsync(UserId));
            return await base.OnGetAsync();
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ValidateModel();

                await AccountAppService.ResetPasswordAsync(
                    new ResetPasswordDto
                    {
                        UserId = UserId,
                        ResetToken = ResetToken,
                        Password = Password
                    }
                );
            }
            catch (AbpIdentityResultException e)
            {
                if (!string.IsNullOrWhiteSpace(e.Message))
                {
                    Alerts.Warning(GetLocalizeExceptionMessage(e));
                    return Page();
                }

                throw;
            }
            catch (AbpValidationException e)
            {
                return Page();
            }

            return RedirectToPage("./ResetPasswordConfirmation");
        }
    }
}
