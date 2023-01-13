using AutoMapper.Internal.Mappers;
using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.JobDashboard
{
    public class CreateModal : JobPortalPageModel
    {
        [BindProperty]
        public CreateJobDto Job { get; set; }

        private readonly IJobAppService _jobAppService;

        public CreateModal(IJobAppService jobAppService)
        {
            _jobAppService = jobAppService;
        }

        public async Task OnGetAsync()
        {
            Job = new CreateJobDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            await _jobAppService.CreateAsync(Job);
            return NoContent();
        }
    }
}