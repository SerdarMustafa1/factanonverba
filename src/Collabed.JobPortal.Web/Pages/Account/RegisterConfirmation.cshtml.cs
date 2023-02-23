using Collabed.JobPortal.Email;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Web.Pages.Account
{
    [AllowAnonymous]
    public class CustomRegisterConfirmationModel : PageModel
    {
        private readonly IdentityUserManager _userManager;
        private readonly EmailService _emailService;
        private readonly IBmtAccountAppService _bmtAccountAppService;
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }
        [BindProperty(SupportsGet = true)]
        public string EmailAddress { get; private set; }

        public CustomRegisterConfirmationModel(IdentityUserManager userManager, EmailService emailService, IBmtAccountAppService bmtAccountAppService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _bmtAccountAppService = bmtAccountAppService;
        }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email.IsNullOrWhiteSpace()) return RedirectToPage("/Index");
            EmailAddress = email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound($"Unable to load user with email '{email}'.");

            // TODO Set to true if you want to display the Account/ConfirmEmail page
            DisplayConfirmAccountLink = false;

            // For local testing purposes 
            if (DisplayConfirmAccountLink)
            {
                EmailConfirmationUrl = await SetEmailConfirmationUrl();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm(Name = nameof(EmailAddress))] string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                EmailAddress = emailAddress?? Request.Form[nameof(EmailAddress)];
            }
            EmailConfirmationUrl = await SetEmailConfirmationUrl();
            await _bmtAccountAppService.SendEmailVerificationRequestAsync(new JobPortal.Account.SendEmailVerificationDto() { Email = EmailAddress, CallbackUrl = EmailConfirmationUrl });
            return Page();
        }

        private async Task<string> SetEmailConfirmationUrl()
        {
            var user = await _userManager.FindByEmailAsync(EmailAddress);
            if (user == null) throw new BusinessException($"Unable to load user with email '{EmailAddress}'.");
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
        }
    }
}
