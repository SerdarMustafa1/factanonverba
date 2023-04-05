using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Post
{
    [Authorize(BmtPermissions.ManageJobs)]
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
        //public IEnumerable<SelectListItem> AvailableSupportedDocuments { get; set; }

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

        [BindProperty]
        [MaxLength(250)]
        public string Skills { get; set; }

        //[BindProperty]
        //public string[] SelectedSupportedDocuments { get; set; }

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

        #region nullable

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
        public double? SalaryMinimum { get; set; }

        [BindProperty]
        public double? SalaryMaximum { get; set; }

        [BindProperty]
        public bool IsSalaryNegotiable { get; set; } = false;

        [BindProperty]
        public int? PositionsAvailable { get; set; }

        [BindProperty]
        public DateTime? ApplicationDeadline { get; set; }

        [BindProperty]
        public string? ScreeningQuestion1 { get; set; }

        [BindProperty]
        public bool? AutoRejectAnswer1 { get; set; }

        [BindProperty]
        public string? ScreeningQuestion2 { get; set; }

        [BindProperty]
        public bool? AutoRejectAnswer2 { get; set; }

        [BindProperty]
        public string? ScreeningQuestion3 { get; set; }

        [BindProperty]
        public bool? AutoRejectAnswer3 { get; set; }

        #endregion

        private readonly DropDownAppService _dropDownService;
        private readonly IJobAppService _jobAppService;

        public JobAdInformationModel(DropDownAppService dropDownService, IJobAppService jobAppService)
        {
            _dropDownService = dropDownService;
            _jobAppService = jobAppService;
        }

        public async Task OnGet()
        {
            JobCategories = (await _dropDownService.GetCategoriesAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            OfficeLocations = (await _dropDownService.GetLocationsAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            Languages = (await _dropDownService.GetLanguagesAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            ExperienceLevels = _dropDownService.GetExperienceLevel().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            EmploymentTypes = _dropDownService.GetEmploymentTypes().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            ContractTypes = _dropDownService.GetContractTypes().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            JobLocations = _dropDownService.GetJobLocations().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            SalaryPeriods = _dropDownService.GetSalaryPeriod().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            //AvailableSupportedDocuments = (await _dropDownService.GetSupporitngDocumentsAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Request.Form["Source"].IsNullOrEmpty() & Request.Form["Source"].ToString() == "JobAdPreview")
            {
                JobCategories = (await _dropDownService.GetCategoriesAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                OfficeLocations = (await _dropDownService.GetLocationsAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                Languages = (await _dropDownService.GetLanguagesAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                ExperienceLevels = _dropDownService.GetExperienceLevel().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                EmploymentTypes = _dropDownService.GetEmploymentTypes().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                ContractTypes = _dropDownService.GetContractTypes().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                JobLocations = _dropDownService.GetJobLocations().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                SalaryPeriods = _dropDownService.GetSalaryPeriod().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                //AvailableSupportedDocuments = (await _dropDownService.GetSupporitngDocumentsAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                return Page();
            }
            else
            {
                return Page();
            }
        }


        private IEnumerable<(string, bool?)> GetScreeningQuestions()
        {
            var result = new List<(string, bool?)>();
            if (!string.IsNullOrWhiteSpace(ScreeningQuestion1))
            {
                result.Add((ScreeningQuestion1, AutoRejectAnswer1 ?? null));
            }
            if (!string.IsNullOrWhiteSpace(ScreeningQuestion2))
            {
                result.Add((ScreeningQuestion2, AutoRejectAnswer2 ?? null));
            }
            if (!string.IsNullOrWhiteSpace(ScreeningQuestion3))
            {
                result.Add((ScreeningQuestion3, AutoRejectAnswer3 ?? null));
            }

            return result;
        }
    }
}
