using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Types;
using Collabed.JobPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.PermissionManagement;

namespace Collabed.JobPortal.Web.Pages.Applications
{
    [Authorize(BmtPermissions.ManageJobs)]
    public class ApplicationsModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IPermissionManager _permissionManager;
        public int AllApplicationsCount { get; set; }
        public int InterviewCount { get; set; }
        public int ReviewCount { get; set; }
        public int FinalDecisionCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string Tab { get; set; } = "all";
        [BindProperty(SupportsGet = true)]
        public string Reference { get; set; }
        public ApplicationModel ApplicationModel { get; set; } = new ApplicationModel();
        public JobSummary JobDetails { get; set; } = new JobSummary();

        public ApplicationsModel(IJobAppService jobAppService)
        {
            _jobAppService=jobAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Reference))
                return NotFound();

            var jobDto = await _jobAppService.GetByReferenceWithApplicationsAsync(Reference);
            JobDetails = ObjectMapper.Map<JobDto, JobSummary>(jobDto);
            ApplicationModel.CurrentPage = CurrentPage;
            ApplicationModel.Tab = Tab;
            ApplicationModel.Reference = Reference;

            var applications = jobDto.Applicants;
            if (applications.Count == 0)
                return Page();

            AllApplicationsCount = applications.Count;
            InterviewCount = applications.Where(x => x.ApplicationStatus == ApplicationStatus.Interview).Count();
            ReviewCount = applications.Where(x => x.ApplicationStatus == ApplicationStatus.Review).Count();
            FinalDecisionCount = applications.Where(x => x.ApplicationStatus == ApplicationStatus.Final).Count();

            var hired = applications.Where(x => x.ApplicationStatus == ApplicationStatus.Hired).Count();
            if (hired >= jobDto.PositionsAvailable)
            {
                ApplicationModel.CanHireApplicants = false;
                JobDetails.PositionsAvailable = 0;
                JobDetails.AcceptingApplications = false;
            }
            else
            {
                ApplicationModel.CanHireApplicants = true;
                JobDetails.PositionsAvailable = jobDto.PositionsAvailable - hired;
            }

            if (jobDto.Status == JobStatus.Closed || (jobDto.ApplicationDeadline - DateTime.Today).TotalDays < 0)
            {
                JobDetails.AcceptingApplications = false;
                JobDetails.PositionsAvailable = 0;
            }

            switch (Tab)
            {
                case "interview":
                    ApplicationModel.Applications = GetApplicationAtStage(applications, ApplicationStatus.Interview);
                    break;
                case "review":
                    ApplicationModel.Applications = GetApplicationAtStage(applications, ApplicationStatus.Review);
                    break;
                case "final":
                    ApplicationModel.Applications = GetApplicationAtStage(applications, ApplicationStatus.Final);
                    break;
                case "all":
                default:
                    ApplicationModel.Applications = GetApplicationAtStage(applications);
                    break;
            }

            return Page();
        }

        private List<Models.Application> GetApplicationAtStage(List<JobApplicationDto> applications, ApplicationStatus? stage = null)
        {
            if (stage != null)
            {
                return applications.Where(x => x.ApplicationStatus == stage).Select(z => new Models.Application
                {
                    JobApplicationId = z.JobApplicantId,
                    ApplicationDate = z.ApplicationDate,
                    InterviewDate = z.InterviewDate,
                    CandidateEmail = z.Email,
                    CandidatePhoneNumber = z.PhoneNumber,
                    CandidateName = $"{z.FirstName} {z.LastName}",
                    ApplicationStatus = z.ApplicationStatus
                }).ToList();
            }

            return applications.Select(z => new Models.Application
            {
                JobApplicationId = z.JobApplicantId,
                ApplicationDate = z.ApplicationDate,
                InterviewDate = z.InterviewDate,
                CandidateEmail = z.Email,
                CandidatePhoneNumber = z.PhoneNumber,
                CandidateName = $"{z.FirstName} {z.LastName}",
                ApplicationStatus = z.ApplicationStatus
            }).ToList();
        }
    }

    public class JobSummary
    {
        public string Title { get; set; }
        public float? SalaryMinimum { get; set; }
        public float? SalaryMaximum { get; set; }
        public string ContractType { get; set; }
        public string EmploymentType { get; set; }
        public string SalaryPeriod { get; set; }
        public string JobLocation { get; set; }
        public string ExperienceLevel { get; set; }
        public bool? HiringMultipleCandidates { get; set; }
        public int? PositionsAvailable { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public bool AcceptingApplications { get; set; } = true;
    }
}
