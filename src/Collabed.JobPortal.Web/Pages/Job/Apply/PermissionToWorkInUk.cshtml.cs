using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Collabed.JobPortal.Web.Helper;
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
            UpdatedStepValue = (string)TempData.Peek(nameof(UpdatedStepValue));
            await LoadPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HasRightToWork.HasValue)
            {
                UpdatedStepValue = "2";
                await LoadPage();
                return Page();
            }

            if (HasRightToWork.HasValue && HasRightToWork.Value == true)
            {
                TempData[nameof(UpdatedStepValue)] = UpdatedStepValue;
                return await NextPage();
            }

            UpdatedStepValue = (string)TempData.Peek(nameof(UpdatedStepValue));
            return RedirectToPage("NoPermission");
        }

        private async Task LoadPage()
        {
            ReadTempData();
            await GetStepsRequired();
            ProgressBarValue = CustomHelper.CalculateProgressBar(StepsRequired, double.Parse(UpdatedStepValue));
        }
    }
}
