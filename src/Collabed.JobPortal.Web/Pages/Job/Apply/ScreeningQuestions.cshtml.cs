using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class ScreeningQuestionsModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;

        public ScreeningQuestionDto Question1 { get; set; }
        public ScreeningQuestionDto Question2 { get; set; }
        public ScreeningQuestionDto Question3 { get; set; }

        [BindProperty]
        public bool? Answer1 { get; set; }
        [BindProperty]
        public bool? Answer2 { get; set; }
        [BindProperty]
        public bool? Answer3 { get; set; }

        public ScreeningQuestionsModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }
        public async Task OnGetAsync()
        {
            TempData[nameof(CurrentStep)] = 3;
            ReadTempData();
            float stepsRequired = (await _jobAppService.GetApplicationStepsByJobReferenceAsync(TempData.Peek("JobReference").ToString())).Value;
            ProgressBarValue = (float.Parse(TempData.Peek(nameof(CurrentStep)).ToString()) / stepsRequired) * 100;
            await AssignQuestions();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await AssignQuestions();
            if (!CheckEmptyAnswers())
            {
                return Page();
            }

            if (ValidateAnsers())
            {
                StoreAnswers();
                return await NextPage();
            }

            return RedirectToPage("WrongScreeningAnswer");
        }

        public int GetStepsRequired()
        {
            return _jobAppService.GetApplicationStepsByJobReferenceAsync(TempData.Peek("JobReference").ToString()).Result.Value;
        }

        private void StoreAnswers()
        {
            if (Answer1.HasValue && Question1 != null)
            {
                TempData["Answer1"] = $"{Question1.Id},{Answer1.Value}";
            }
            if (Answer2.HasValue && Question2 != null)
            {
                TempData["Answer2"] = $"{Question2.Id},{Answer2.Value}";
            }
            if (Answer3.HasValue && Question3 != null)
            {
                TempData["Answer3"] = $"{Question3.Id},{Answer3.Value}";
            }
        }

        private async Task AssignQuestions()
        {
            var questions = await _jobAppService.ScreeningQuestionsByJobRefAsync(TempData.Peek("JobReference")?.ToString());
            Question1 = questions.ElementAtOrDefault(0);
            Question2 = questions.ElementAtOrDefault(1);
            Question3 = questions.ElementAtOrDefault(2);
        }

        private bool CheckEmptyAnswers()
        {
            if (Question1 != null && !Answer1.HasValue)
            {
                ModelState.AddModelError(nameof(Answer1), "");
                return false;
            }
            if (Question2 != null && !Answer2.HasValue)
            {
                ModelState.AddModelError(nameof(Answer2), "");
                return false;
            }
            if (Question3 != null && !Answer3.HasValue)
            {
                ModelState.AddModelError(nameof(Answer3), "");
                return false;
            }
            return true;
        }

        private bool ValidateAnsers()
        {
            if (Answer1.HasValue && Question1 != null && Question1.AutoRejectAnswer.HasValue && Answer1.Value == Question1.AutoRejectAnswer.Value)
            {
                return false;
            }
            if (Answer2.HasValue && Question2 != null && Question2.AutoRejectAnswer.HasValue && Answer2.Value == Question2.AutoRejectAnswer.Value)
            {
                return false;
            }
            if (Answer3.HasValue && Question3 != null && Question3.AutoRejectAnswer.HasValue && Answer3.Value == Question3.AutoRejectAnswer.Value)
            {
                return false;
            }
            return true;
        }
    }
}
