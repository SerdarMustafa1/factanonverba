using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Collabed.JobPortal.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class CoverLetterModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;

        [BindProperty, Required]
        public string CoverLetter { get; set; }

        public CoverLetterModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }

        public async Task OnGetAsync()
        {
            TempData[nameof(CurrentStep)] = 6;
            UpdatedStepValue = (string)TempData.Peek(nameof(UpdatedStepValue));
            ReadTempData();
            await GetStepsRequired();
            ProgressBarValue = CustomHelper.CalculateProgressBar(StepsRequired, double.Parse(UpdatedStepValue));
            var jobTitle = TempData.Peek("Title");
            CoverLetter = $"I have just viewed your job vacancy for {jobTitle} on BuildMyTalent and would like to be considered for this position. \r\n\r\nPlease find a copy of my CV attached.\r\n";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TempData[nameof(CoverLetter)] = CoverLetter;
            return await NextPage();
        }
    }
}
