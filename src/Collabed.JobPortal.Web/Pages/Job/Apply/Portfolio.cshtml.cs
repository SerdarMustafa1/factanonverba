using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
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
            ReadTempData();

            await GetStepsRequired();
            ProgressBarValue = CalculateProgressBar(StepsRequired, CurrentStep);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TempData[nameof(PortfolioLink)] = PortfolioLink;
            return await NextPage();
        }
    }
}
