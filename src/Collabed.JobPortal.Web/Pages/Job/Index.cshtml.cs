using Collabed.Application.Helpers;
using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public IEnumerable<string> Category { get; set; }

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
            return SalaryRangeHelper.GetSalaryRange(JobDto.SalaryMinimum, JobDto.SalaryMaximum, JobDto.IsSalaryEstimated);
        }
    }
}
