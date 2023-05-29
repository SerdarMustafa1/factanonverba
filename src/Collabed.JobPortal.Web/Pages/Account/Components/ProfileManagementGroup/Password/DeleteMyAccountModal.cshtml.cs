using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.User;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.Password
{
    public class DeleteMyAccountModal : AbpPageModel
    {
        [BindProperty]
        public Guid UserId { get; set; }
        [BindProperty]
        public Guid CompanyId { get; set; }
        [BindProperty]
        public string CompanyName { get; set; }

        private readonly IOrganisationAppService _organisationAppService;
        private readonly IBmtAccountAppService _bmtAccountAppService;

        public DeleteMyAccountModal(IOrganisationAppService organisationAppService, IBmtAccountAppService bmtAccountAppService)
        {
            _organisationAppService = organisationAppService;
            _bmtAccountAppService = bmtAccountAppService;
        }

        public async Task OnGetAsync()
        {
        }

        public async Task OnPostAsync()
        {
            var currentUserId = CurrentUser.Id;
            var userType = await _bmtAccountAppService.GetUserTypeByIdAsync(currentUserId.Value);

            if (userType == UserType.Candidate)
            {
                await _bmtAccountAppService.DeleteUserAsync(currentUserId.Value);
                return;
            }

            if (userType == UserType.Organisation)
            {
                var organisationClaim = CurrentUser.FindClaim(ClaimNames.OrganisationClaim);
                if (organisationClaim != null && !string.IsNullOrEmpty(organisationClaim.Value))
                {
                    var organisationId = Guid.Parse(organisationClaim.Value);
                    await _organisationAppService.DeleteAllOrganisationDataAsync(organisationId);
                }
                return;
            }
        }
    }
}