using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class SuccessModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;
        public SuccessModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }

        public void OnGet()
        {
            
        }
    }
}
