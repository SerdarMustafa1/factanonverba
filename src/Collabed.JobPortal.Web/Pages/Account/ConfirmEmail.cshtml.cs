using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Collabed.JobPortal.Web.Pages.Account
{
    [AllowAnonymous]
    public class CustomConfirmEmailModel : PageModel
    {
        private readonly IdentityUserManager _userManager;
        private readonly IOrganisationRepository _organisationRepository;
        public bool IsOrganisation { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public string DisplayName { get; set; }

        public CustomConfirmEmailModel(IdentityUserManager userManager, IOrganisationRepository organisationRepository)
        {
            _userManager = userManager;
            _organisationRepository = organisationRepository;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId.IsNullOrWhiteSpace() || code.IsNullOrWhiteSpace())
                return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            var organisationClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(claim => claim.Type.Equals(ClaimNames.OrganisationClaim));

            if (organisationClaim != null && !string.IsNullOrEmpty(organisationClaim.Value))
            {
                var org = await _organisationRepository.GetAsync(o => o.Id.Equals(Guid.Parse(organisationClaim.Value)));
                DisplayName = org.Name;
                IsOrganisation = true;
            }
            else
            {
                DisplayName = user.Name;
                IsOrganisation = false;
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                // Redirect to page to send confirmation email again
            }

            return Page();
        }
    }
}