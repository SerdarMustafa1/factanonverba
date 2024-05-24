using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class PermissionToWorkInUkModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;

        [BindProperty, Required]
        public bool? HasRightToWork { get; set; }

        public PermissionToWorkInUkModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }

        public async Task OnGetAsync()
        {
            TempData[nameof(CurrentStep)] = 2;
            ReadTempData();
            await GetStepsRequired();
            ProgressBarValue = CalculateProgressBar(StepsRequired, CurrentStep);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HasRightToWork.HasValue && HasRightToWork.Value == true)
            {
                return await NextPage();
            }

            return RedirectToPage("NoPermission");
        }
    }
}
