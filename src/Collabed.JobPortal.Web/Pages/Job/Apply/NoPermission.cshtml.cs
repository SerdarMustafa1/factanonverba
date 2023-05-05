using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class NoPermissionModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;
        public NoPermissionModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }
    }
}
