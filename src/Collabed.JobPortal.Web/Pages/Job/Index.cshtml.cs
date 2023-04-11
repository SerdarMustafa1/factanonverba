using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job
{
    [BindProperties(SupportsGet = true)]
    public class JobModel : AbpPageModel
    {
        public string Reference { get; set; }

        /// <summary>
        /// Search params from jobDashboard page, to have smooth UX on after clicking back to results link
        /// Inserted directly after last endpoint, so it needs to be followed by / and other endpoints (if neccesary) 
        /// or question marks (if some param is necessary)
        /// </summary>
        public string PreviousParams { get; set; }

        public JobDto JobDto { get; set; }

        private readonly IJobAppService _jobAppService;

        public JobModel(IJobAppService jobAppService)
        {
            _jobAppService = jobAppService;
        }

        public async Task OnGet()
        {
            JobDto = await _jobAppService.GetByReferenceAsync(Reference);
        }

        public string GetSalaryRange()
        {
            if (JobDto.SalaryMinimum.HasValue && JobDto.SalaryMinimum.Value > 0
                && JobDto.SalaryMaximum.HasValue && JobDto.SalaryMaximum.Value > 0)
            {
                return $"£{JobDto.SalaryMinimum.Value:N2} - £{JobDto.SalaryMaximum.Value:N2}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
