using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class UploadCvModel : ApplyForAJobModelBase
    {
        private readonly IJobAppService _jobAppService;
        private readonly IBmtAccountAppService _accountAppService;
        private readonly IWebHostEnvironment _env;

        [BindProperty]
        //[ConditionalRequiredIfBothEmpty(PropertyName1 = nameof(StoredCvFileName), PropertyName2 = nameof(StoredCvContentType), ErrorMessage = "Please select a resume")]
        public IFormFile Upload { get; set; }

        public string StoredCvContentType { get; set; }
        public string StoredCvFileName { get; set; }

        public UploadCvModel(IJobAppService jobAppService, IBmtAccountAppService accountAppService, IWebHostEnvironment env) :
            base(jobAppService, accountAppService)
        {
            _jobAppService = jobAppService;
            _accountAppService = accountAppService;
            _env = env;
        }
        public async Task OnGetAsync()
        {
            TempData[nameof(CurrentStep)] = 4;
            ReadTempData();
            JobDto = await _jobAppService.GetByReferenceAsync(TempData.Peek(nameof(JobReference))?.ToString());
            await AssignProgressBar();
            var accountProfile = await _accountAppService.GetLoggedUserProfileAsync();
            if (!string.IsNullOrWhiteSpace(accountProfile.CvFileName) &&
                !string.IsNullOrWhiteSpace(accountProfile.CvContentType))
            {
                TempData["CvFileName"] = accountProfile.CvFileName;
                TempData["CvContentType"] = accountProfile.CvContentType;
                StoredCvFileName = accountProfile.CvFileName;
                StoredCvContentType = accountProfile.CvContentType;
            }
            TempData["UserId"] = accountProfile.UserId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            StoredCvFileName = TempData.Peek("CvFileName")?.ToString();
            StoredCvContentType = TempData.Peek("CvContentType")?.ToString();
            // Check if both StoredCvFileName and StoredCvContentType are empty and Upload is not provided
            if (string.IsNullOrWhiteSpace(StoredCvFileName) && string.IsNullOrWhiteSpace(StoredCvContentType) && Upload == null)
            {
                ModelState.AddModelError(nameof(Upload), "Please select a resume");
                await AssignProgressBar();
                return Page();
            }

            var userId = Guid.Parse(TempData.Peek("UserId")?.ToString());
            var cvFileName = TempData.Peek("CvFileName")?.ToString();
            var cvContentType = TempData.Peek("CvContentType")?.ToString();
            var uploadingNewCv = string.IsNullOrWhiteSpace(cvFileName) && string.IsNullOrWhiteSpace(cvContentType) && Upload != null ||
                !string.IsNullOrWhiteSpace(cvFileName) && !string.IsNullOrWhiteSpace(cvContentType) && Upload != null && !Upload.FileName.Equals(cvFileName);
            if (uploadingNewCv)
            {
                cvFileName = Upload.FileName;
                cvContentType = Upload.ContentType;
                using var memoryStream = new CustomMemoryStream();
                await Upload.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                await _accountAppService.UploadCvToUserProfile(userId, memoryStream, cvFileName, cvContentType);
            }
            return await NextPage();
        }
        private async Task AssignProgressBar()
        {
            float stepsRequired = (await _jobAppService.GetApplicationStepsByJobReferenceAsync(TempData.Peek("JobReference").ToString())).Value;
            ProgressBarValue = (float.Parse(TempData.Peek(nameof(CurrentStep)).ToString()) / stepsRequired) * 100;
        }
    }
}
