using Collabed.JobPortal.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Web.Pages.Account
{
    [AllowAnonymous]
    public class CustomRegisterConfirmationModel : PageModel
    {
        private readonly IdentityUserManager _userManager;
        private readonly EmailService _emailService;
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }

        public CustomRegisterConfirmationModel(IdentityUserManager userManager, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email.IsNullOrWhiteSpace()) return RedirectToPage("/Index");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound($"Unable to load user with email '{email}'.");

            // TODO Set to true if you want to display the Account/ConfirmEmail page
            DisplayConfirmAccountLink = false;

            // For local testing purposes 
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = userId, code = code },
                    protocol: Request.Scheme);
            }
            return Page();
        }
    }
}
