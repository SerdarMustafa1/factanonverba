using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Clients.Modals
{
    [Authorize(BmtPermissions.Admin)]
    public class DeleteClientModal : AbpPageModel
    {
        [BindProperty]
        public Guid UserId { get; set; }
        [BindProperty]
        public Guid CompanyId { get; set; }
        [BindProperty]
        public string CompanyName { get; set; }

        private readonly IOrganisationAppService _organisationAppService;

        public DeleteClientModal(IOrganisationAppService organisationAppService)
        {
            _organisationAppService = organisationAppService;
        }

        public async Task OnGetAsync(string userId, string companyId, string companyName)
        {
            if (Guid.TryParse(userId, out var userIdGuid))
                UserId = userIdGuid;

            if (Guid.TryParse(companyId, out var companyIdGuid))
                CompanyId = companyIdGuid;
            CompanyName = companyName;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _organisationAppService.DeleteAllOrganisationDataAsync(CompanyId);
            return NoContent();
        }
    }
}
