using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.JobDashboard
{
    public class JobDashboardModel : PageModel
    {
        private readonly IJobAppService _jobAppService;
        public CategorisedJobsDto[] Categories;

        public JobDashboardModel(IJobAppService jobAppService)
        {
            _jobAppService = jobAppService;
        }

        public async Task OnGetAsync()
        {
            Categories = (await _jobAppService.GetCategorisedJobs()).ToArray();
        }
    }
}
