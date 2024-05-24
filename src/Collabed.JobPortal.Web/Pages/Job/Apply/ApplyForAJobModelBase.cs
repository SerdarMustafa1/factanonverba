using Collabed.Application.Helpers;
using Collabed.JobPortal.Applications;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class ApplyForAJobModelBase : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;

        #region searchParams
        /// <summary>
        /// Search params from jobDashboard page, to have smooth UX on after clicking back to results link
        /// Inserted directly after last endpoint, so it needs to be followed by / and other endpoints (if neccesary) 
        /// or question marks (if some param is necessary)
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public IEnumerable<string> Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Predicate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Location { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedRadius { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Sorting { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        #endregion

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [BindProperty]
        [RegularExpression("^[^@]+@[^@]+(\\.[^@]+)*\\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }

        [BindProperty]
        [RegularExpression("^(\\d{10})$", ErrorMessage = "Please enter a valid UK Phone Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your phone number")]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string PostCode { get; set; }

        /// <summary>
        /// Describes the step number in apply for a job flow: 1) enter contact details, 2) answer screening questions, 3) upload CV, 4) point out portfolio, 5) add cover letter
        /// </summary>
        public int CurrentStep { get; set; }
        public string JobReference { get; set; }
        public int StepsRequired { get; set; }
        public float ProgressBarValue { get; set; }
        public bool ScreeningQuestionsExists { get; set; }
        public bool IsCvRequired { get; set; }
        public bool IsCoverLetterRequired { get; set; }
        public bool IsPortfolioRequired { get; set; }
        public IEnumerable<SupportingDocumentDto> RequiredDocuments { get; set; }
        public JobDto JobDto { get; set; }

        public ApplyForAJobModelBase(IJobAppService jobAppService, IBmtAccountAppService accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }

        public void ReadTempData()
        {
            FirstName = (string)TempData.Peek(nameof(FirstName));
            LastName = (string)TempData.Peek(nameof(LastName));
            EmailAddress = (string)TempData.Peek(nameof(EmailAddress));
            PhoneNumber = (string)TempData.Peek(nameof(PhoneNumber));
            CurrentStep = int.Parse(TempData.Peek(nameof(CurrentStep)).ToString());
            ScreeningQuestionsExists = bool.Parse(TempData.Peek(nameof(ScreeningQuestionsExists)).ToString());
            IsCvRequired = bool.Parse(TempData.Peek(nameof(IsCvRequired)).ToString());
            IsCoverLetterRequired = bool.Parse(TempData.Peek(nameof(IsCoverLetterRequired)).ToString());
            IsPortfolioRequired = bool.Parse(TempData.Peek(nameof(IsPortfolioRequired)).ToString());
        }

        public void UpdateTempData()
        {
            TempData[nameof(IsCvRequired)] = IsCvRequired;
            TempData[nameof(IsCoverLetterRequired)] = IsCoverLetterRequired;
            TempData[nameof(IsPortfolioRequired)] = IsPortfolioRequired;
            TempData[nameof(FirstName)] = FirstName;
            TempData[nameof(LastName)] = LastName;
            TempData[nameof(EmailAddress)] = EmailAddress;
            TempData[nameof(PhoneNumber)] = PhoneNumber;
            TempData[nameof(ProgressBarValue)] = ProgressBarValue;
            TempData[nameof(CurrentStep)] = CurrentStep;
        }

        public async Task<IActionResult> NextPage()
        {
            ReadTempData();
            // if ScreeningQuestionExists => first go to ScreeningQuestions
            // but if CurrentStep is other than 1, then go to UploadCV
            // Unless, it is not a supported document, then go to Portfolio,
            // Unless, it is not a supported document, then go to CoverLetter,
            switch (CurrentStep)
            {
                case 1:
                    return RedirectToPage("PermissionToWorkInUk");
                case 2:
                    if (ScreeningQuestionsExists)
                        return RedirectToPage("ScreeningQuestions");
                    else if (IsCvRequired)
                        return RedirectToPage("UploadCv");
                    else if (IsPortfolioRequired)
                        return RedirectToPage("Portfolio");
                    else if (IsCoverLetterRequired)
                        return RedirectToPage("CoverLetter");
                    break;
                case 3:
                    if (IsCvRequired)
                        return RedirectToPage("UploadCv");
                    else if (IsPortfolioRequired)
                        return RedirectToPage("Portfolio");
                    else if (IsCoverLetterRequired)
                        return RedirectToPage("CoverLetter");
                    break;
                case 4:
                    if (IsPortfolioRequired)
                        return RedirectToPage("Portfolio");
                    else if (IsCoverLetterRequired)
                        return RedirectToPage("CoverLetter");
                    break;
                case 5:
                    if (IsCoverLetterRequired)
                        return RedirectToPage("CoverLetter");
                    break;
                default:
                    break;
            }

            var answers = ExtractScreeningAnswers();
            var application = new ApplicationDto()
            {
                CoverLetter = TempData.Peek("CoverLetter")?.ToString(),
                UserId = CurrentUser.Id != null ? CurrentUser.Id.Value : Guid.Parse(TempData.Peek("UserId").ToString()),
                Email = TempData.Peek(nameof(EmailAddress))?.ToString(),
                FirstName = TempData.Peek(nameof(FirstName))?.ToString(),
                LastName = TempData.Peek(nameof(LastName))?.ToString(),
                JobReference = TempData.Peek(nameof(JobReference))?.ToString(),
                PhoneNumber = TempData.Peek(nameof(PhoneNumber))?.ToString(),
                PostCode = TempData.Peek(nameof(PostCode))?.ToString(),
                CvContentType = TempData.Peek(nameof(ApplicationDto.CvContentType))?.ToString(),
                CvFileName = TempData.Peek(nameof(ApplicationDto.CvFileName))?.ToString(),
                PortfolioLink = TempData.Peek(nameof(ApplicationDto.PortfolioLink))?.ToString(),
                IsNewCvAttached = false,
                ScreeningQuestions = answers
            };
            await _jobAppService.ApplyForAJob(application,
                TempData.Peek("Password")?.ToString());
            return RedirectToPage("Success");
        }

        private List<ScreeningQuestionDto> ExtractScreeningAnswers()
        {
            var result = new List<ScreeningQuestionDto>();

            if (TempData.TryGetValue("Answer1", out var question1))
            {
                var answer1 = JsonConvert.DeserializeObject<ScreeningQuestionDto>((string)question1);
                result.Add(answer1);
            }
            if (TempData.TryGetValue("Answer2", out var question2))
            {
                var answer2 = JsonConvert.DeserializeObject<ScreeningQuestionDto>((string)question2);
                result.Add(answer2);
            }
            if (TempData.TryGetValue("Answer3", out var question3))
            {
                var answer3 = JsonConvert.DeserializeObject<ScreeningQuestionDto>((string)question3);
                result.Add(answer3);
            }

            //var answer2 = TempData.Peek("Answer2")?.ToString();
            //var answer3 = TempData.Peek("Answer3")?.ToString();

            //if (answer1 != null)
            //    result.Add(new ScreeningQuestionDto(System.Guid.Parse(answer1.Split(',')[0]), "") { Answer = bool.Parse(answer1.Split(',')[1]) });
            //if (answer2 != null)
            //    result.Add(new ScreeningQuestionDto(System.Guid.Parse(answer2.Split(',')[0]), "") { Answer = bool.Parse(answer2.Split(',')[1]) });
            //if (answer3 != null)
            //    result.Add(new ScreeningQuestionDto(System.Guid.Parse(answer3.Split(',')[0]), "") { Answer = bool.Parse(answer3.Split(',')[1]) });

            return result;
        }

        public string GetSalaryRange()
        {
            return SalaryRangeHelper.GetSalaryRange(JobDto.SalaryMinimum, JobDto.SalaryMaximum, JobDto.IsSalaryEstimated);
        }

        public async Task GetStepsRequired()
        {
            StepsRequired = (await _jobAppService.GetApplicationStepsByJobReferenceAsync(TempData.Peek(nameof(JobReference))?.ToString())).Value;
        }

        public int CalculateProgressBar(double totalSteps, double currentStep)
        {
            var progressBarValue = Math.Round(currentStep / totalSteps * 100, 0);

            return (int)progressBarValue;
        }
    }
}
