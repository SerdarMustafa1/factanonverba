using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Collabed.JobPortal.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class UploadPortfolioModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;

        [BindProperty, Required(ErrorMessage = "Please type your portfolio's address")]
        public string PortfolioLink { get; set; }

        public UploadPortfolioModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) :
            base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService; _accountAppService = accountAppService;
        }
        public async Task OnGetAsync()
        {
            TempData[nameof(CurrentStep)] = 5;
            UpdatedStepValue = (string)TempData.Peek(nameof(UpdatedStepValue));
            ReadTempData();

            await GetStepsRequired();
            ProgressBarValue = CustomHelper.CalculateProgressBar(StepsRequired, double.Parse(UpdatedStepValue));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TempData[nameof(PortfolioLink)] = PortfolioLink;
            TempData[nameof(UpdatedStepValue)] = UpdatedStepValue;
            return await NextPage();
        }
    }
}
