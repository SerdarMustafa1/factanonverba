using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Collabed.JobPortal.Jobs;

namespace Collabed.JobPortal.Web.Pages.JobDashboard
{
    public class EditModal : JobPortalPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateJobDto EditingJob { get; set; }

        private readonly IJobAppService _jobAppService;

        public EditModal(IJobAppService jobAppService)
        {
            _jobAppService = jobAppService;
        }

        public async Task OnGetAsync()
        {
            var jobDto = await _jobAppService.GetAsync(Id);
            EditingJob = ObjectMapper.Map<JobDto, CreateUpdateJobDto>(jobDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            // await _jobAppService.UpdateAsync(Id, EditingJob);
            return NoContent();
        }
    }
}
