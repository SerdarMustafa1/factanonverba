using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.PermissionManagement;

namespace Collabed.JobPortal.Web.Pages.JobListings
{
    [Authorize(BmtPermissions.ManageJobs)]
    public class JobListingsModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IPermissionManager _permissionManager;
        public int TotalCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public List<JobListing> JobListings { get; set; }
        public bool CanPostNewJob { get; set; }

        public JobListingsModel(IJobAppService jobAppService, IPermissionManager permissionManager)
        {
            _jobAppService = jobAppService;
            _permissionManager = permissionManager;
        }

        public async Task OnGetAsync()
        {
            await GetJobListingsAsync(CurrentPage);
        }

        public void OnPostSubmit()
        {
        }

        private async Task GetJobListingsAsync(int page)
        {
            var organisationId = ExtractOrganisationId();

            var listings = await _jobAppService.GetListAsync(new JobGetListInput
            {
                OrganisationId = organisationId,
                MaxResultCount =  PageSize,
                SkipCount = (page - 1)* PageSize
            });

            JobListings = listings.Items.Select(x => new JobListing
            {
                Reference = x.Reference,
                Title = x.Title
            }).ToList();
            TotalCount = (int)listings.TotalCount;

            var permissions = await _permissionManager.GetAllForUserAsync((Guid)CurrentUser.Id);

            if (permissions.Any(x => x.Name == BmtPermissions.PostJobs && x.IsGranted))
            {
                CanPostNewJob = true;
            }
        }

        private Guid ExtractOrganisationId()
        {
            var organisationClaim = CurrentUser.FindClaim(ClaimNames.OrganisationClaim);
            if (organisationClaim == null || string.IsNullOrEmpty(organisationClaim.Value))
            {
                throw new BusinessException(message: "User is not allowed to post jobs. User is not a member of any organisation.");
            }
            var organisationId = Guid.Parse(organisationClaim.Value);
            return organisationId;
        }
    }

    public class JobListing
    {
        public string Reference { get; set; }
        public string Title { get; set; }
    }
}