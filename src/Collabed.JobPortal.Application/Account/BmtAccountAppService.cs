using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(BmtAccountAppService), typeof(IAccountAppService), typeof(AccountAppService), typeof(IBmtAccountAppService))]
    public class BmtAccountAppService : AccountAppService, IBmtAccountAppService
    {
        private readonly IBmtAccountEmailer _accountEmailer;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IFileAppService _fileAppService;
        private readonly ILogger<BmtAccountAppService> _logger;
        private readonly UserManager _profileUserManager;

        public BmtAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IBmtAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions,
            IRepository<UserProfile> userProfileRepository,
            IFileAppService fileAppService,
            UserManager profileUserManager,
            ILogger<BmtAccountAppService> logger) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _accountEmailer = accountEmailer;
            _userProfileRepository = userProfileRepository;
            _fileAppService = fileAppService;
            _profileUserManager = profileUserManager;
            _logger = logger;
        }

        [Authorize(BmtPermissions.ApplyForJobs)]
        public async Task<UserProfileDto> GetLoggedUserProfileAsync()
        {
            if (CurrentUser == null)
            {
                throw new UserFriendlyException("User must be logged in");
            }

            var userProfileDto = new UserProfileDto
            {
                FirstName = CurrentUser.Name,
                LastName = CurrentUser.SurName,
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber,
                UserId = CurrentUser.Id.Value,
            };

            var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == CurrentUser.Id);
            if (userProfile == null)
            {
                return userProfileDto;
            }

            userProfileDto.CvBlobName = userProfile.CvBlobName;
            userProfileDto.CvFileName= userProfile.CvFileName;
            userProfileDto.CvContentType = userProfile.CvContentType;
            userProfileDto.PostCode = userProfile.PostCode;

            return userProfileDto;
        }

        public async Task UploadCvToUserProfile(Guid UserId, Stream fileStream, string fileName, string contentType)
        {
            var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == UserId);
            if (userProfile != null)
            {
                if (!string.IsNullOrWhiteSpace(userProfile.CvBlobName))
                {
                    var blobDeleted = await _fileAppService.DeleteBlobAsync(userProfile.CvBlobName);
                    _logger.LogInformation($"Attempted to delete CV blob '{userProfile.CvBlobName}' for user with Id: {UserId} with result: {blobDeleted}");
                }
            }
            else
            {
                userProfile = _profileUserManager.CreateUserProfile(UserId);
                userProfile = await _userProfileRepository.InsertAsync(userProfile, true);
            }

            var blobFileName = RandomNameGenerator.GenerateRandomName(10);
            userProfile.CvBlobName = blobFileName;
            userProfile.CvContentType = contentType;
            userProfile.CvFileName = fileName;

            try
            {
                await _fileAppService.SaveBlobAsync(new SaveBlobInputDto { Name = blobFileName, Content = ReadFully(fileStream) });
                await _userProfileRepository.UpdateAsync(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occured during CV upload for user {UserId}: {ex.Message}");
                _logger.LogException(ex);
            }
        }

        public override async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            var userDto = await base.RegisterAsync(input);

            return userDto;
        }

        public async Task SendEmailVerificationRequestAsync(SendEmailVerificationDto input)
        {
            var user = await GetUserByEmailAsync(input.Email);
            await _accountEmailer.SendEmailVerificationRequestAsync(user, input.CallbackUrl);
        }

        public async Task<bool> CheckIfEmailExistsAsync(string emailAddress)
        {
            return (await UserManager.FindByEmailAsync(emailAddress) != null);
        }

        public async Task<bool> CheckIfUsernameExistsAsync(string userName)
        {
            return (await UserManager.FindByNameAsync(userName) != null);
        }

        public async Task<bool> CheckPasswordCredentials(string loginInput, string password)
        {
            var isEmail = await CheckIfEmailExistsAsync(loginInput);
            if (isEmail)
            {
                var user = await UserManager.FindByEmailAsync(loginInput);
                return await UserManager.CheckPasswordAsync(user, password);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(loginInput);
                return user != null ? await UserManager.CheckPasswordAsync(user, password) : false;
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            using MemoryStream ms = new();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
