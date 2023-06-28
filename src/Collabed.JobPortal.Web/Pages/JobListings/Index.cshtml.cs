using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Types;
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
    public class ListingsModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IPermissionManager _permissionManager;
        public bool CanPostNewJob { get; set; } = true;
        public int AllJobsCount { get; set; }
        public int LiveCount { get; set; }
        public int HiringCount { get; set; }
        public int ClosedCount { get; set; }
        public int TotalCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string Tab { get; set; } = "all";
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        public List<Listing> AllJobListings { get; set; } = new List<Listing>();
        //public List<Listing> LiveJobListings { get; set; } = new List<Listing>();
        //public List<Listing> HiringJobListings { get; set; } = new List<Listing>();
        //public List<Listing> ClosedJobListings { get; set; } = new List<Listing>();

        public ListingsModel(IJobAppService jobAppService, IPermissionManager permissionManager)
        {
            _jobAppService = jobAppService;
            _permissionManager = permissionManager;
        }

        public async Task OnGetAsync()
        {
            await SearchJobListingsAsync();
        }

        private async Task SearchJobListingsAsync()
        {
            var organisationId = ExtractOrganisationId();

            var getJobsInput = new JobGetListInput
            {
                OrganisationId = organisationId,
                SearchCriteria = Search,
                Status = GetStatus(Tab),
                SkipCount = CurrentPage == 1 ? 0 : (CurrentPage-1) * PageSize,
                MaxResultCount = PageSize
            };

            if (!string.IsNullOrWhiteSpace(Search))
            {
                getJobsInput.SearchCriteria = Search;
            }

            var jobListings = await _jobAppService.GetAllListAsync(getJobsInput);

            if (jobListings.Items.Any())
            {
                AllJobListings = jobListings.Items.Select(ObjectMapper.Map<JobSummaryDto, Listing>).ToList();
            }

            var jobsByStatus = await _jobAppService.GetStatusedJobsCount(getJobsInput);
            AllJobsCount = jobsByStatus.AllJobsCount;
            LiveCount = jobsByStatus.LiveCount;
            HiringCount = jobsByStatus.HiringCount;
            ClosedCount = jobsByStatus.ClosedCount;

            var permissions = await _permissionManager.GetAllForUserAsync((Guid)CurrentUser.Id);
            if (permissions.Any(x => x.Name == BmtPermissions.PostJobs && x.IsGranted))
            {
                CanPostNewJob = true;
            }
        }

        public async Task OnPostAsync()
        {
            await SearchJobListingsAsync();
        }

        private static JobStatus? GetStatus(string tab)
        {
            return tab switch
            {
                "all" => null,
                "live" => JobStatus.Live,
                "hiring" => JobStatus.Hiring,
                "closed" => JobStatus.Closed,
                _ => null,
            };
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

    public class Listing
    {
        public string Reference { get; set; }
        public string Title { get; set; }
        public int ApplicationsCount { get; set; }
        public int InProcessCount { get; set; }
        public int HiredCount { get; set; }
        public string DaysLeft { get; set; }
        public JobStatus Status { get; set; }

        public string DisplayCount(int count)
        {
            if (count == default)
                return "-";

            return count.ToString();
        }
    }
}
