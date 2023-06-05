using Collabed.Application.Helpers;
using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Types;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job.Post
{
    [Authorize(BmtPermissions.PostJobs)]
    public class PreviewModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly DropDownAppService _dropDownAppService;

        public PreviewModel(IJobAppService jobAppService, IOrganisationRepository organisationRepository, DropDownAppService dropDownAppService)
        {
            _jobAppService= jobAppService;
            _dropDownAppService= dropDownAppService;
            _organisationRepository = organisationRepository;
        }

        #region Required props
        [BindProperty]
        public string JobTitle { get; set; }
        [BindProperty]
        public string JobDescription { get; set; }
        [BindProperty]
        public string SubDescription { get; set; }
        [BindProperty]
        public int JobCategoryId { get; set; }
        [BindProperty]
        public int EmploymentTypeId { get; set; }
        [BindProperty]
        public int ContractTypeId { get; set; }
        [BindProperty]
        public int JobLocationTypeId { get; set; }
        #endregion

        [BindProperty]
        public string Skills { get; set; }

        //[BindProperty]
        //public string SelectedSupportedDocuments { get; set; }

        [BindProperty]
        public string SupplementalPay { get; set; }

        [BindProperty]
        public string OtherCompanyBenefits { get; set; }

        [BindProperty]
        public bool IsCvRequired { get; set; }

        [BindProperty]
        public bool IsCoverLetterRequired { get; set; }

        [BindProperty]
        public bool IsOnlinePortfolioRequired { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ExperienceLevelId { get; set; }

        [BindProperty]
        public int? JobLocationId { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public int? CandidateExperienceLevel { get; set; }

        [BindProperty]
        public bool? LocalLanguageRequired { get; set; }

        [BindProperty]
        public int? LanguageId { get; set; } = 1;

        [BindProperty]
        public bool? OfferingVisaSponsorship { get; set; }

        [BindProperty]
        public bool? IsNetZeroCompliant { get; set; }

        [BindProperty]
        public int? SalaryPeriodId { get; set; }

        [BindProperty]
        public float? SalaryMinimum { get; set; }

        [BindProperty]
        public float? SalaryMaximum { get; set; }

        [BindProperty]
        public bool? IsSalaryNegotiable { get; set; }

        [BindProperty]
        public int? PositionsAvailable { get; set; }

        [BindProperty]
        public DateTime? ApplicationDeadline { get; set; }

        [BindProperty]
        public string ScreeningQuestion1 { get; set; }

        [BindProperty]
        public bool? DesiredAnswer1 { get; set; }

        [BindProperty]
        public string ScreeningQuestion2 { get; set; }

        [BindProperty]
        public bool? DesiredAnswer2 { get; set; }

        [BindProperty]
        public string ScreeningQuestion3 { get; set; }

        [BindProperty]
        public bool? DesiredAnswer3 { get; set; }

        public string GetOrganisationName()
        {
            var organisationClaim = CurrentUser.FindClaim(ClaimNames.OrganisationClaim);
            if (organisationClaim != null && !string.IsNullOrEmpty(organisationClaim.Value))
            {
                var organisationId = Guid.Parse(organisationClaim.Value);
                var organisation = _organisationRepository.GetAsync(x => x.Id == organisationId).Result;
                if (organisation != null)
                {
                    return organisation.Name;
                }
            }
            return string.Empty;
        }

        public string GetLocalLanguageName()
        {
            var res = _dropDownAppService.GetLanguageNameById(LanguageId.Value).Result;
            if (res != null)
            {
                return res;
            }
            return string.Empty;
        }

        public string GetSalaryPeriodName()
        {
            if (SalaryPeriodId.HasValue)
            {
                // HACK TODO shortcuts
                return Enum.GetName(typeof(SalaryPeriod), SalaryPeriodId);
            }
            return string.Empty;
        }

        public string GetSalaryRange()
        {
            return SalaryRangeHelper.GetSalaryRange(SalaryMinimum, SalaryMaximum, false);
        }

        public string GetCurrentDate()
        {
            var suffix = "";

            if (DateTime.Now.Day == 1 ||
                DateTime.Now.Day == 21 ||
                DateTime.Now.Day == 31)
                suffix = "st";
            else if (DateTime.Now.Day == 2 ||
                DateTime.Now.Day == 22)
                suffix = "nd";
            else if (DateTime.Now.Day == 3 ||
                DateTime.Now.Day == 23)
                suffix = "rd";
            else
                suffix = "th";

            return DateTime.Now.ToString($"d'\\{suffix[0]}\\{suffix[1]}' MMMM yyyy");
        }

        public string GetContractType()
        {
            return Enum.Parse<ContractType>(ContractTypeId.ToString()).GetDisplayName();
        }

        public string GetEmploymentType()
        {
            return Enum.Parse<EmploymentType>(EmploymentTypeId.ToString()).GetDisplayName();
        }

        public string GetJobLocationType()
        {
            return Enum.Parse<JobLocation>(JobLocationTypeId.ToString()).GetDisplayName();
        }

        public string GetOfficeLocation()
        {
            if (!JobLocationId.HasValue)
                return string.Empty;

            return _dropDownAppService.GetLocationByIdAsync(JobLocationId.Value).Result;
        }

        public string GetExperienceLevel()
        {
            if (!ExperienceLevelId.HasValue)
                return string.Empty;

            return Enum.GetName(Enum.Parse<ExperienceLevel>(ExperienceLevelId.ToString()));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Request.Form["Source"].IsNullOrEmpty() && Request.Form["Source"].ToString() == "JobAdInformation")
            {
                return Page();
            }

            var createdJob = new CreateJobDto()
            {
                ApplicationDeadline = ApplicationDeadline,
                CategoryId = JobCategoryId,
                ContractType = (ContractType)ContractTypeId,
                Description = JobDescription,
                EmploymentType = (EmploymentType)EmploymentTypeId,
                ExperienceLevel = ExperienceLevelId != null ? (ExperienceLevel)ExperienceLevelId : null,
                IsLocalLanguageRequired = LocalLanguageRequired,
                IsSalaryNegotiable = IsSalaryNegotiable,
                JobLocation = (JobLocation)JobLocationTypeId,
                LocalLanguageId = LanguageId,
                OfficeLocationId = JobLocationId,
                PaymentOption = SalaryPeriodId != null ? (SalaryPeriod)SalaryPeriodId : null,
                PositionsAvailable = PositionsAvailable,
                SalaryCurrency = JobPortal.Job.CurrencyType.GBP,
                SalaryOtherBenefits = OtherCompanyBenefits,
                Skills = Skills,
                StartDate = StartDate,
                SubDescription = SubDescription,
                SupplementalPay = SupplementalPay,
                ScreeningQuestions = GetScreeningQuestions(),
                SupportingDocuments = GetRequiredDocuments(),
                Title = JobTitle,
                IsNetZeroCompliant = IsNetZeroCompliant
            };
            if (!IsSalaryNegotiable.HasValue || !IsSalaryNegotiable.Value)
            {
                createdJob.SalaryMaximum = SalaryMaximum != null ? SalaryMaximum : null;
                createdJob.SalaryMinimum = SalaryMinimum != null ? SalaryMinimum : null;
            }
            // HACK: Implement Suporting Documents appropriatelly and add Screening questions 
            var job = await _jobAppService.CreateAsync(createdJob);

            return RedirectToPage($"../Index", new { job.Reference });
        }

        private ICollection<int> GetRequiredDocuments()
        {
            var result = new List<int>();
            if (IsCvRequired)
                result.Add(1);
            if (IsCoverLetterRequired)
                result.Add(3);
            if (IsOnlinePortfolioRequired)
                result.Add(5);
            return result;
        }

        private IEnumerable<(string, bool?)> GetScreeningQuestions()
        {
            var screeningQuestionsList = new List<Models.ScreeningQuestion>();
            if (ScreeningQuestion1 != null)
                screeningQuestionsList.Add(new Models.ScreeningQuestion(ScreeningQuestion1, !DesiredAnswer1));
            if (ScreeningQuestion2 != null)
                screeningQuestionsList.Add(new Models.ScreeningQuestion(ScreeningQuestion2, !DesiredAnswer2));
            if (ScreeningQuestion3 != null)
                screeningQuestionsList.Add(new Models.ScreeningQuestion(ScreeningQuestion3, !DesiredAnswer3));

            var screeningQuestionTuples = new (string, bool?)[screeningQuestionsList.Count];

            for (int i = 0; i < screeningQuestionsList.Count; i++)
            {
                screeningQuestionTuples[i] = new(screeningQuestionsList[i].Question, screeningQuestionsList[i].AutoRejectAnswer);
            }

            return screeningQuestionTuples;
        }
    }
}
