using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class WrongScreeningAnswerModel : ApplyForAJobModelAbstract
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;

        public WrongScreeningAnswerModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService) : 
            base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            return await NextPage();
        }
    }
}
