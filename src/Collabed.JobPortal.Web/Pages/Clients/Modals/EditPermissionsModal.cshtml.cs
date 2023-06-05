using Collabed.JobPortal.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace Collabed.JobPortal.Web.Pages.Clients.Modals
{
    [Authorize(BmtPermissions.Admin)]
    public class EditPermissionsModal : AbpPageModel
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IdentityUserManager _userManager;

        [BindProperty]
        public bool JobPostingPermission { get; set; }
        [BindProperty]
        public Guid UserId { get; set; }
        public EditPermissionsModal(IPermissionManager permissionManager, IdentityUserManager userManager)
        {
            _permissionManager = permissionManager;
            _userManager = userManager;
        }

        public async Task OnGetAsync(string userId, string jobPostingPermission)
        {
            if (Guid.TryParse(userId, out var userIdGuid))
                UserId = userIdGuid;

            JobPostingPermission = jobPostingPermission == "True" ? true : false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UserId == default)
                return NoContent();

            //var user = await _userManager.GetByIdAsync(UserId);
            //var roles = user.Roles.Any(x => x.);
            //await _userManager.AddToRoleAsync(user, RoleNames.JobManager);
            await _permissionManager.SetForUserAsync(UserId, BmtPermissions.PostJobs, JobPostingPermission);

            return new ObjectResult(JobPostingPermission);
        }
    }
}
