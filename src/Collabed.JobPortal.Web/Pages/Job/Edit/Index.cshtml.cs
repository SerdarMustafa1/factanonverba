using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Collabed.JobPortal.Attributes;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Types;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job.Edit
{
    [Authorize(BmtPermissions.PostJobs)]
    public class JobAdInformationModel : PageModel
    {
        public IEnumerable<SelectListItem> JobCategories { get; set; }
        public IEnumerable<SelectListItem> OfficeLocations { get; set; }
        public IEnumerable<SelectListItem> Languages { get; set; }
        public IEnumerable<SelectListItem> ExperienceLevels { get; set; }
        public IEnumerable<SelectListItem> EmploymentTypes { get; set; }
        public IEnumerable<SelectListItem> ContractTypes { get; set; }
        public IEnumerable<SelectListItem> JobLocations { get; set; }
        public IEnumerable<SelectListItem> SalaryPeriods { get; set; }

        #region required

        [BindProperty]
        [Required(ErrorMessage = "Please enter a job title")]
        [MaxLength(63)]
        public string JobTitle { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a job description")]
        public string JobDescription { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please provide a sub-description")]
        [MaxLength(190)]
        public string SubDescription { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a category from the dropdown")]
        public int JobCategoryId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select an employment type from the dropdown")]
        public int EmploymentTypeId { get; set; } = -1;

        [BindProperty]
        [Required(ErrorMessage = "Please select a contract type from the dropdown")]
        public int ContractTypeId { get; set; } = -1;

        [BindProperty]
        [Required(ErrorMessage = "Please select a job location type from the dropdown")]
        public int JobLocationTypeId { get; set; } = -1;

        #endregion

        [BindProperty(SupportsGet = true)] public string Reference { get; set; }

        [BindProperty] [MaxLength(250)] public string Skills { get; set; }

        [BindProperty] public string SupplementalPay { get; set; }

        [BindProperty] public string OtherCompanyBenefits { get; set; }

        [BindProperty] public bool IsCvRequired { get; set; }

        [BindProperty] public bool IsCoverLetterRequired { get; set; }

        [BindProperty] public bool IsOnlinePortfolioRequired { get; set; }

        #region nullable

        [BindProperty(SupportsGet = true)] public int? ExperienceLevelId { get; set; }

        [BindProperty] public int? JobLocationId { get; set; }

        [BindProperty] public DateTime? StartDate { get; set; }

        [BindProperty] public int? CandidateExperienceLevel { get; set; }

        [BindProperty] public bool? LocalLanguageRequired { get; set; } = false;

        [BindProperty] public int? LanguageId { get; set; }

        [BindProperty] public bool? OfferingVisaSponsorship { get; set; }

        [BindProperty] public bool? IsNetZeroCompliant { get; set; } = false;

        [BindProperty] public int? SalaryPeriodId { get; set; }

        [BindProperty] public double? SalaryMinimum { get; set; }

        [BindProperty] public double? SalaryMaximum { get; set; }

        [BindProperty] public bool IsSalaryNegotiable { get; set; } = false;

        [BindProperty] public int? PositionsAvailable { get; set; } = 1;

        [BindProperty] public DateTime? ApplicationDeadline { get; set; }

        [BindProperty] public string? ScreeningQuestion1 { get; set; }

        [BindProperty] public bool? DesiredAnswer1 { get; set; }

        [BindProperty] public string? ScreeningQuestion2 { get; set; }

        [BindProperty] public bool? DesiredAnswer2 { get; set; }

        [BindProperty] public string? ScreeningQuestion3 { get; set; }

        [BindProperty] public bool? DesiredAnswer3 { get; set; }

        #endregion

        public string MaxDateTime { get; set; }

        private readonly DropDownAppService _dropDownService;
        private readonly IJobAppService _jobAppService;

        public JobAdInformationModel(DropDownAppService dropDownService, IJobAppService jobAppService)
        {
            _dropDownService = dropDownService;
            _jobAppService = jobAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Reference))
            {
                return NotFound();
            }

            var job = await _jobAppService.GetByReferenceAsync(Reference);
            if (job == null)
            {
                return NotFound();
            }

            MaxDateTime = job.PublishedDate.AddDays(30).ToString("yyyy-MM-dd");
            var jobDocuments = (await _jobAppService.GetSupportingDocumentsByJobRefAsync(Reference)).ToList();
            var jobScreenings = await _jobAppService.ScreeningQuestionsByJobRefAsync(Reference);
            Languages = (await _dropDownService.GetLanguagesAsync()).Select(x =>
                new SelectListItem(x.Name, x.Id.ToString()));
            JobCategories =
                (await _dropDownService.GetCategoriesAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            OfficeLocations =
                (await _dropDownService.GetLocationsAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            ExperienceLevels = _dropDownService.GetExperienceLevel()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            EmploymentTypes = _dropDownService.GetEmploymentTypes()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            ContractTypes = _dropDownService.GetContractTypes()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            JobLocations = _dropDownService.GetJobLocations().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            SalaryPeriods = _dropDownService.GetSalaryPeriod().Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            JobTitle = job.Title;
            SubDescription = job.SubDescription;
            JobDescription = job.Description;
            JobCategoryId = job.CategoryId == 0 ? int.Parse(JobCategories.First().Value) : job.CategoryId;
            Skills = job.Skills;
            EmploymentTypeId = (int)job.EmploymentType.GetValueFromName<EmploymentType>();
            ContractTypeId = (int)Enum.Parse<ContractType>(job.ContractType);
            JobLocationTypeId = (int)job.JobLocation.GetValueFromName<JobLocation>();
            JobLocationId = job.OfficeLocation != null ? int.Parse(OfficeLocations.First(x => x.Text.Equals(job.OfficeLocation)).Value) : null;
            StartDate = job.StartDate;
            ExperienceLevelId = job.ExperienceLevel != null ? (int)job.ExperienceLevel.GetValueFromName<ExperienceLevel>() : null;
            LocalLanguageRequired = job.IsLocalLanguageRequired ?? false;
            LanguageId = job.LocalLanguage != null ? int.Parse(Languages.First(x => x.Text.Equals(job.LocalLanguage)).Value) : null;
            IsNetZeroCompliant = job.IsNetZeroCompliant;
            SalaryPeriodId = job.SalaryPeriod != null ? (int)job.SalaryPeriod.GetValueFromName<SalaryPeriod>() : null;
            SalaryMinimum = job.SalaryMinimum;
            SalaryMaximum = job.SalaryMaximum;
            IsSalaryNegotiable = job.IsSalaryNegotiable ?? false;
            SupplementalPay = job.SupplementalPay;
            OtherCompanyBenefits = job.SalaryBenefits;
            IsCvRequired = jobDocuments.Any(d => d.Name.Equals("CV"));
            IsCoverLetterRequired = jobDocuments.Any(d => d.Name.Equals("Cover Letter"));
            IsOnlinePortfolioRequired = jobDocuments.Any(d => d.Name.Equals("Online Portfolio"));
            PositionsAvailable = job.PositionsAvailable;
            ApplicationDeadline = job.ApplicationDeadline.Date;
            var question = 1;
            foreach (var screeningQuestionDto in jobScreenings)
            {
                switch (question)
                {
                    case 1:
                        ScreeningQuestion1 = screeningQuestionDto.Text;
                        DesiredAnswer1 = !screeningQuestionDto.AutoRejectAnswer;
                        break;
                    case 2:
                        ScreeningQuestion2 = screeningQuestionDto.Text;
                        DesiredAnswer2 = !screeningQuestionDto.AutoRejectAnswer;
                        break;
                    case 3:
                        ScreeningQuestion3 = screeningQuestionDto.Text;
                        DesiredAnswer3 = !screeningQuestionDto.AutoRejectAnswer;
                        break;
                }

                question++;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Request.Form["Source"].IsNullOrEmpty() & Request.Form["Source"].ToString() == "Preview")
            {
                JobCategories =
                    (await _dropDownService.GetCategoriesAsync()).Select(x =>
                        new SelectListItem(x.Name, x.Id.ToString()));
                OfficeLocations =
                    (await _dropDownService.GetLocationsAsync()).Select(
                        x => new SelectListItem(x.Name, x.Id.ToString()));
                Languages = (await _dropDownService.GetLanguagesAsync()).Select(x =>
                    new SelectListItem(x.Name, x.Id.ToString()));
                ExperienceLevels = _dropDownService.GetExperienceLevel()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                EmploymentTypes = _dropDownService.GetEmploymentTypes()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                ContractTypes = _dropDownService.GetContractTypes()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                JobLocations = _dropDownService.GetJobLocations()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                SalaryPeriods = _dropDownService.GetSalaryPeriod()
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            }

            return Page();
        }
    }
}