using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Post
{
    public class JobAdPreviewModel : PageModel
    {

        private readonly IJobAppService _jobAppService;

        public JobAdPreviewModel(IJobAppService jobAppService)
        {
            _jobAppService= jobAppService;
        }
        #region required

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
        public double? SalaryMinimum { get; set; }

        [BindProperty]
        public double? SalaryMaximum { get; set; }

        [BindProperty]
        public bool? IsSalaryNegotiable { get; set; }

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

        public string GetContractType()
        {
            return Enum.GetName(Enum.Parse<ContractType>(ContractTypeId.ToString()));
        }

        public string GetEmploymentType()
        {
            return Enum.GetName(Enum.Parse<EmploymentType>(EmploymentTypeId.ToString()));
        }

        public string GetJobCategory()
        {
            throw new NotImplementedException("Add a service, this comes from the DB");
            //return Enum.GetName(Enum.Parse<JobCategory>)
        }

        public string GetJobLocationType()
        {
            return Enum.GetName(Enum.Parse<JobLocation>(JobLocationTypeId.ToString()));
        }

        public string GetOfficeLocation()
        {
            throw new NotImplementedException("Add a service, this comes from the DB");
        }

        public string GetExperienceLevel()
        {
            return Enum.GetName(Enum.Parse<ExperienceLevel>(ExperienceLevelId.ToString()));
        }

        public string GetSalaryRange()
        {
            if (SalaryPeriodId != null)
            {
                // HACK TODO
            }
            else
            {
                // HACK TODO
            }
            throw new NotImplementedException();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Request.Form["Source"].IsNullOrEmpty() & Request.Form["Source"].ToString() == "JobAdInformation")
            {
                return Page();
            }
            else
            {
                // fix wrong input checkboxes on JobAdInformation.cshtml
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
                    OfferVisaSponsorship = OfferingVisaSponsorship,
                    OfficeLocationId = JobLocationId,
                    PaymentOption = SalaryPeriodId != null ? (SalaryPeriod)SalaryPeriodId : null,
                    PositionsAvailable = PositionsAvailable,
                    SalaryCurrency = JobPortal.Job.CurrencyType.GBP,
                    SalaryMaximum = SalaryMaximum != null ? (float)SalaryMaximum : null,
                    SalaryMinimum = SalaryMinimum != null ? (float)SalaryMinimum : null,
                    SalaryOtherBenefits = OtherCompanyBenefits,
                    Skills = Skills,
                    StartDate = StartDate,
                    SubDescription = SubDescription,
                    SupplementalPay = SupplementalPay,
                    SupportingDocuments = new List<int>(),// SelectedSupportedDocuments.Select(int.Parse).ToList(),
                    Title = JobTitle,
                    IsNetZeroCompliant = IsNetZeroCompliant
                };
                await _jobAppService.CreateAsync(createdJob);
                return RedirectToPage("/Index");
            }
        }
    }
}
