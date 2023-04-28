using Collabed.Application.Helpers;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class ContactDetailsModel : ApplyForAJobModelAbstract
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;
        public ContactDetailsModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService; 
        }

        public async Task OnGetAsync([FromQuery] string jobReference)
        {
            TempData.Clear();
            JobReference = jobReference;
            TempData[nameof(JobReference)] = JobReference;
            JobDto = await _jobAppService.GetByReferenceAsync(JobReference);
            var userProfile = await _accountAppService.GetLoggedUserProfileAsync();
            if(userProfile != null)
            {
                if (!string.IsNullOrWhiteSpace(userProfile.Email)) EmailAddress = userProfile.Email;
                if (!string.IsNullOrWhiteSpace(userProfile.FirstName)) FirstName = userProfile.FirstName;
                if (!string.IsNullOrWhiteSpace(userProfile.LastName)) LastName = userProfile.LastName;
                if (!string.IsNullOrWhiteSpace(userProfile.PhoneNumber)) PhoneNumber = userProfile.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(userProfile.PostCode)) PostCode = userProfile.PostCode;
            }
            TempData[nameof(CurrentStep)] = 1;
            TempData[nameof(ScreeningQuestionsExists)] = (await  _jobAppService.ScreeningQuestionsByJobRefAsync(JobReference)).Count() > 0;
            TempData["Title"] = JobDto.Title;
            TempData["SalaryRange"] = GetSalaryRange();
            TempData["OrganisationName"] = JobDto.OrganisationName;
            TempData["PostedDate"] = JobDto.PublishedDate.ToString("dd/MM/yyyy");
            TempData[nameof(JobDto.EmploymentType)] = JobDto.EmploymentType;
            TempData[nameof(JobDto.ContractType)] = JobDto.ContractType;
            TempData[nameof(JobDto.OfficeLocation)] = JobDto.OfficeLocation;
            TempData[nameof(JobDto.JobLocation)] = JobDto.JobLocation;
            TempData[nameof(JobDto.PublishedDate)] = JobDto.PublishedDate;
            float stepsRequired = (await _jobAppService.GetApplicationStepsByJobReferenceAsync(TempData.Peek("JobReference").ToString())).Value;
            ProgressBarValue = (float.Parse(TempData.Peek(nameof(CurrentStep)).ToString()) / stepsRequired) * 100;
            await AssignRequiredDocumentsToTempData();
            AssignUrlParamsToTempData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var temporary = this;
            if (ModelState.IsValid)
            {
                TempData[nameof(EmailAddress)] = EmailAddress;
                TempData[nameof(FirstName)] = FirstName;
                TempData[nameof(LastName)] = LastName;
                TempData[nameof(PhoneNumber)] = PhoneNumber;
                TempData[nameof(PostCode)] = PostCode;
                return await NextPage();
            }
            else
            {
                return Page();
            }
        }



        private async Task AssignRequiredDocumentsToTempData()
        {
            RequiredDocuments = await _jobAppService.GetSupportingDocumentsByJobRefAsync(JobReference);
            if (JobDto.JobOrigin != Types.JobOrigin.Native || RequiredDocuments.FirstOrDefault(i => i.Name.Contains("CV", StringComparison.InvariantCultureIgnoreCase)) != null) TempData[nameof(IsCvRequired)] = true;
            else TempData[nameof(IsCvRequired)] = false;
            if (JobDto.JobOrigin != Types.JobOrigin.Native || RequiredDocuments.FirstOrDefault(i => i.Name.Contains("Cover Letter", StringComparison.InvariantCultureIgnoreCase)) != null) TempData[nameof(IsCoverLetterRequired)] = true;
            else TempData[nameof(IsCoverLetterRequired)] = false;
            if (JobDto.JobOrigin != Types.JobOrigin.Native || RequiredDocuments.FirstOrDefault(i => i.Name.Contains("Portfolio", StringComparison.InvariantCultureIgnoreCase)) != null) TempData[nameof(IsPortfolioRequired)] = true;
            else TempData[nameof(IsPortfolioRequired)] = false;
        }

        private void AssignUrlParamsToTempData()
        {
            var categories = "";
            foreach (var item in Category)
            {
                categories = $"{categories}Category={item}&";
            }
            TempData["BackToJobOfferParams"] = $"../../Job?Reference={JobDto.Reference}&{categories}{nameof(Predicate)}={Predicate}&{nameof(Location)}={Location}&{nameof(SelectedRadius)}={SelectedRadius}&{nameof(Sorting)}={Sorting}&{nameof(CurrentPage)}={CurrentPage}";

        }
    }
}
