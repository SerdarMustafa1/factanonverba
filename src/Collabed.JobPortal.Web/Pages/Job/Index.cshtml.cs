using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job
{
    [BindProperties(SupportsGet = true)]
    public class JobModel : PageModel
    {
        public string Reference { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private readonly IJobAppService _jobAppService;

        public JobModel(IJobAppService jobAppService)
        {
            _jobAppService = jobAppService;
        }

        public async Task OnGet(string jobReference)
        {
            var job = await _jobAppService.GetByReferenceAsync(jobReference);
            if (job != null)
            {
                Reference = job.Reference;
                Title = job.Title;
                Description = job.Description;
            }
        }
    }
}
