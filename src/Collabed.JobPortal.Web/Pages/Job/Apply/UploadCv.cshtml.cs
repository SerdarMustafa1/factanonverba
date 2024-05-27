using Collabed.JobPortal.Account;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Users;
using Collabed.JobPortal.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Identity;

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

        public UploadCvModel(IJobAppService jobAppService,
            IBmtAccountAppService accountAppService,
            IWebHostEnvironment env) :
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
            UpdatedStepValue = (string)TempData.Peek(nameof(UpdatedStepValue));
            JobDto = await _jobAppService.GetByReferenceAsync(TempData.Peek(nameof(JobReference))?.ToString());
            await GetStepsRequired();

            ProgressBarValue = CustomHelper.CalculateProgressBar(StepsRequired, CurrentStep);

            var accountProfile = await _accountAppService.GetLoggedUserProfileAsync();
            if (accountProfile != null)
            {
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = new Guid();

            StoredCvFileName = TempData.Peek("CvFileName")?.ToString();
            StoredCvContentType = TempData.Peek("CvContentType")?.ToString();
            TempData[nameof(UpdatedStepValue)] = UpdatedStepValue;
            // Check if both StoredCvFileName and StoredCvContentType are empty and Upload is not provided
            if (string.IsNullOrWhiteSpace(StoredCvFileName) && string.IsNullOrWhiteSpace(StoredCvContentType) && Upload == null)
            {
                ModelState.AddModelError(nameof(Upload), "Please select a resume");
                await GetStepsRequired();
                ProgressBarValue = CustomHelper.CalculateProgressBar(StepsRequired, double.Parse(UpdatedStepValue));
                return Page();
            }

            if (TempData.Peek("UserId") == null)
            {
                var user = await CreateUser();

                if (user != null)
                {
                    userId = (Guid)user.Id;
                    TempData["UserId"] = userId;
                    TempData["Password"] = user.Password; //TODO: CHANGE LATER
                }
            }
            else
            {
                userId = Guid.Parse(TempData.Peek("UserId")?.ToString());
            }

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

        private async Task<RegisterUserDto> CreateUser()
        {
            var resgisterUser = new RegisterUserDto()
            {
                Email = TempData.Peek("EmailAddress").ToString(),
                Name = TempData.Peek("FirstName").ToString(),
                Surname = TempData.Peek("LastName").ToString(),
                UserName = TempData.Peek("EmailAddress").ToString()
            };

            var userDto = new IdentityUserDto();
            var registerUserDto = new RegisterUserDto();

            userDto = await _accountAppService.GetRegisteredUserByEmailAsync(resgisterUser.Email);

            if (userDto == null)
            {
                registerUserDto = await _accountAppService.RegisterLocalUserAsync(JobPortal.User.UserType.Candidate, resgisterUser);
            }
            else
            {
                registerUserDto.Email = userDto.Email;
                registerUserDto.Surname = userDto.Surname;
                registerUserDto.UserId = userDto.Id;
                registerUserDto.Id = userDto.Id;
                registerUserDto.UserName = userDto.UserName;
                registerUserDto.PhoneNumber = userDto.PhoneNumber;
                registerUserDto.Name = userDto.Name;
            }


            return registerUserDto;
        }
    }
}
