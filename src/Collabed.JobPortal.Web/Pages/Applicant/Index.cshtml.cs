using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Applicant
{
    [Authorize(BmtPermissions.ManageJobs)]
    public class ApplicantModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IFileAppService _fileAppService;

        [BindProperty(SupportsGet = true)]
        public string Reference { get; set; }
        public string JobReference { get; set; }
        [BindProperty]
        public Applicant Applicant { get; set; }
        public IEnumerable<SelectListItem> Ratings { get; }
        public ApplicantModel(IJobAppService jobAppService, IFileAppService fileAppService)
        {
            _jobAppService = jobAppService;
            _fileAppService = fileAppService;
            Ratings  = new SelectListItem[] {
                new SelectListItem("1", "1", true),
                new SelectListItem("2", "2", true),
                new SelectListItem("3", "3", true),
                new SelectListItem("4", "4", true),
                new SelectListItem("5", "5", true)
            };
        }

        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrWhiteSpace(Reference))
            {
                return NotFound();
            }
            var applicant = await _jobAppService.GetApplicationByReferenceAsync(Reference);
            if (applicant == null)
            {
                return NotFound();
            }
            JobReference = applicant.JobReference;

            Applicant = new Applicant
            {
                FullName = $"{applicant.FirstName} {applicant.LastName}",
                Email = applicant.Email,
                Phone = $"+44 {applicant.PhoneNumber}",
                PostCode = applicant.PostCode,
                Cv = applicant.CvFileName,
                CvBlobName = applicant.CvBlobFileName,
                CvContentType = applicant.CvContentType,
                CoverLetter = applicant.CoverLetter,
                Portfolio = applicant.PortfolioLink,
                ApplicationDate = applicant.ApplicationDate,
                JobTitle = applicant.JobTitle,
                InterviewDate = applicant.InterviewDate,
                Rating = applicant.Rating.HasValue ? applicant.Rating.Value : default,
                Status = applicant.Status,
                ApplicationId = applicant.ApplicationId
            };

            return Page();
        }

        public async Task<ActionResult> OnGetDownloadCvAsync(string fileName, string blobName, string contentType)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(contentType))
            {
                return NoContent();
            }

            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name = blobName });
            if (fileDto == null)
            {
                return NoContent();
            }

            return File(fileDto.Content, contentType, fileName);
        }
    }

    public class Applicant
    {
        public Guid ApplicationId { get; set; }
        public string JobTitle { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime ApplicationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? InterviewDate { get; set; }
        public string PostCode { get; set; }
        public string Portfolio { get; set; }
        public string Cv { get; set; }
        public string CvBlobName { get; set; }
        public string CoverLetter { get; set; }
        public string CvContentType { get; set; }
        public int Rating { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
