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
using Collabed.JobPortal.Attributes;
using JetBrains.Annotations;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Applications
{
    [Authorize(BmtPermissions.ManageJobs)]
    public class ApplicationsModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;

        public int AllApplicationsCount { get; set; }
        public int InterviewCount { get; set; }
        public int ReviewCount { get; set; }
        public int FinalDecisionCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string Tab { get; set; } = "all";
        [BindProperty(SupportsGet = true)] 
        [CanBeNull] public string SortBy { get; set; }
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
            ApplicationModel.SortBy = SortBy;
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
                    ApplicationModel.Applications = GetApplicationAtStage(applications, ApplicationStatus.Interview, GetOrderBy(SortBy));
                    break;
                case "review":
                    ApplicationModel.Applications = GetApplicationAtStage(applications, ApplicationStatus.Review, GetOrderBy(SortBy));
                    break;
                case "final":
                    ApplicationModel.Applications = GetApplicationAtStage(applications, ApplicationStatus.Final, GetOrderBy(SortBy));
                    break;
                case "all":
                default:
                    ApplicationModel.Applications = GetApplicationAtStage(applications, null, GetOrderBy(SortBy));
                    break;
            }

            return Page();
        }

        private List<Models.Application> GetApplicationAtStage(List<JobApplicationDto> applications, ApplicationStatus? stage = null, (string, string)? order = null)
        {
            List<Models.Application> results;
            if (stage != null)
            {
                results = applications.Where(x => x.ApplicationStatus == stage).Select(z => new Models.Application
                {
                    Reference = z.Reference,
                    JobApplicationId = z.JobApplicantId,
                    ApplicationDate = z.ApplicationDate,
                    InterviewDate = z.InterviewDate,
                    CandidateEmail = z.Email,
                    CandidatePhoneNumber = z.PhoneNumber,
                    CandidateName = $"{z.FirstName} {z.LastName}",
                    ApplicationStatus = z.ApplicationStatus,
                    Rating = z.Rating ?? default,
                }).ToList();
            }
            else
            {

                results = applications.Select(z => new Models.Application
                {
                    Reference = z.Reference,
                    JobApplicationId = z.JobApplicantId,
                    ApplicationDate = z.ApplicationDate,
                    InterviewDate = z.InterviewDate,
                    CandidateEmail = z.Email,
                    CandidatePhoneNumber = z.PhoneNumber,
                    CandidateName = $"{z.FirstName} {z.LastName}",
                    ApplicationStatus = z.ApplicationStatus,
                    Rating = z.Rating ?? default,
                }).ToList();
            }

            if (order == null)
            {
                return results;
            }

            return order.Value.Item2 switch
            {
                "asc" => order.Value.Item1 switch
                {
                    "Candidate" => results.OrderBy(application => application.CandidateName).ToList(),
                    "Applied" => results.OrderBy(application => application.ApplicationDate).ToList(),
                    "Rating" => results.OrderBy(application => application.Rating).ToList(),
                    "Status" => results.OrderBy(application => OrderHelper.GetOrder(application.ApplicationStatus)).ToList(),
                    _ => results
                },
                "desc" => order.Value.Item1 switch
                {
                    "Candidate" => results.OrderByDescending(application => application.CandidateName).ToList(),
                    "Applied" => results.OrderByDescending(application => application.ApplicationDate).ToList(),
                    "Rating" => results.OrderByDescending(application => application.Rating).ToList(),
                    "Status" => results.OrderByDescending(application => OrderHelper.GetOrder(application.ApplicationStatus)).ToList(),
                    _ => results
                },
                _ => results
            };
        }
    
    
        [CanBeNull]
        private (string, string)? GetOrderBy(string orderBy)
        {
            switch (orderBy)
            {
                case "CandidateAsc":
                case "CandidateDesc":
                    return ("Candidate", orderBy.EndsWith("Asc") ? "asc" : "desc");
                case "AppliedAsc":
                case "AppliedDesc":
                    return ("Applied", orderBy.EndsWith("Asc") ? "asc" : "desc");
                case "RatingAsc":
                case "RatingDesc":
                    return ("Rating", orderBy.EndsWith("Asc") ? "asc" : "desc");
                case "StatusAsc":
                case "StatusDesc":
                    return ("Status", orderBy.EndsWith("Asc") ? "asc" : "desc");
            }

            return null;
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
