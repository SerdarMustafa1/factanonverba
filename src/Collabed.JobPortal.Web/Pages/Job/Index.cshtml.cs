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

        #region searchParams
        /// <summary>
        /// Search params from jobDashboard page, to have smooth UX on after clicking back to results link
        /// Inserted directly after last endpoint, so it needs to be followed by / and other endpoints (if neccesary) 
        /// or question marks (if some param is necessary)
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Predicate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Location { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedRadius { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Sorting { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        #endregion

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
            if(JobDto.SalaryMinimum.HasValue && JobDto.SalaryMinimum.Value > 0 && JobDto.SalaryMaximum.HasValue && JobDto.SalaryMaximum.Value == JobDto.SalaryMinimum.Value)
            {
                return $"£{JobDto.SalaryMinimum.Value:N2}";
            }
            else if (JobDto.SalaryMinimum.HasValue && JobDto.SalaryMinimum.Value > 0
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
