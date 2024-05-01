using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Helper;
using Collabed.JobPortal.Roles;
using Collabed.JobPortal.User;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Volo.Abp.UI.Navigation.DefaultMenuNames.Application;

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
        private readonly IdentityUserManager _identityUserManager;

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
            _identityUserManager = userManager;
        }

        public async Task<UserType> GetUserTypeByIdAsync(Guid userId)
        {
            var user = await _identityUserManager.GetByIdAsync(userId);
            return (UserType)Enum.Parse(typeof(UserType), user.GetUserType(), true);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == userId);
            if (userProfile != null)
            {
                if (!string.IsNullOrWhiteSpace(userProfile.CvBlobName))
                    await _fileAppService.DeleteBlobAsync(userProfile.CvBlobName);

                await _userProfileRepository.DeleteAsync(userProfile);
            }
            var user = await _identityUserManager.GetByIdAsync(userId);
            await _identityUserManager.DeleteAsync(user);
        }

        public async Task<UserProfileDto> GetLoggedUserProfileAsync()
        {
            var userProfileDto = new UserProfileDto();

            if (CurrentUser.Id != null)
            {
                userProfileDto.FirstName = CurrentUser.Name;
                userProfileDto.LastName = CurrentUser.SurName;
                userProfileDto.Email = CurrentUser.Email;
                userProfileDto.PhoneNumber = CurrentUser.PhoneNumber;
                userProfileDto.UserId = CurrentUser.Id.Value;

                var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == CurrentUser.Id);
                if (userProfile == null)
                {
                    return userProfileDto;
                }

                userProfileDto.CvBlobName = userProfile.CvBlobName;
                userProfileDto.CvFileName = userProfile.CvFileName;
                userProfileDto.CvContentType = userProfile.CvContentType;
                userProfileDto.PostCode = userProfile.PostCode;
                
                return userProfileDto;
            }

            return null;
        }

        public async Task UpdateUserProfileAsync(UpdateUserProfileDto updateProfileDto)
        {
            var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == updateProfileDto.Id);
            if (userProfile == null)
            {
                await _userProfileRepository.InsertAsync(userProfile);
                return;
            }
            userProfile.PostCode = updateProfileDto.PostCode;
            await _userProfileRepository.UpdateAsync(userProfile);
        }

        public async Task<UserProfileDto> GetUserProfileByIdAsync(Guid userId)
        {
            var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == userId);
            if (userProfile == null)
            {
                return default;
            }

            return ObjectMapper.Map<UserProfile, UserProfileDto>(userProfile);
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

        public override async Task<IdentityUserDto> RegisterAsync(Volo.Abp.Account.RegisterDto input)
        {
            var userDto = await base.RegisterAsync(input);

            return userDto;
        }

        public async Task SendEmailVerificationRequestAsync(SendEmailVerificationDto input)
        {
            var user = await GetUserByEmailAsync(input.Email);
            await _accountEmailer.SendEmailVerificationRequestAsync(user, input.CallbackUrl);
        }

        public async Task SendEmailVerificationInJobApplicationRequestAsync(SendEmailVerificationDto input,string password)
        {
            var user = await GetUserByEmailAsync(input.Email);
            await _accountEmailer.SendEmailVerificationInJobApplicationRequestAsync(user, input.CallbackUrl, password);
        }

        public async Task<IdentityUserDto> GetRegisteredUserByEmailAsync(string emailAddress)
        {
            var user = await UserManager.FindByEmailAsync(emailAddress);

            if (user == null)
                return null;

            var userDto = new IdentityUserDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };
            return userDto;
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

        public async Task<RegisterUserDto> RegisterLocalUserAsync(UserType userType, RegisterUserDto registerDto)
        {
            var password = PasswordHelper.GenerateRandomPassword();
            var voloRegisterDto = new Volo.Abp.Account.RegisterDto()
            {
                AppName = "MVC",
                EmailAddress = registerDto.Email,
                Password = password,
                UserName = registerDto.UserName
            };

            var userDto = await RegisterAsync(voloRegisterDto);
            var user = await UserManager.GetByIdAsync(userDto.Id);
            user.Name = registerDto.Name;
            user.Surname = registerDto.Surname;
            await UserManager.UpdateAsync(user);


            await AssignDefaultRoles(userType, user);

            // Send user an email to confirm email address
            await SendEmailToAskForEmailConfirmationAsync(user, password);

            registerDto.Email = userDto.Email;
            registerDto.Surname = userDto.Surname;
            registerDto.UserId = userDto.Id;
            registerDto.Id = userDto.Id;
            registerDto.UserName = userDto.UserName;
            registerDto.PhoneNumber = userDto.PhoneNumber;
            registerDto.Name = userDto.Name;
            registerDto.Password = password;

            return registerDto;
        }
        private async Task SendEmailToAskForEmailConfirmationAsync(IdentityUser user, string password)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //TODO TEST
            //var callbackUrl = _urlHelper.Page("/Account/ConfirmEmail", pageHandler: null, values: new { userId = user.Id, code = code }, protocol: "Scheme");

            var baseUrl = "https://jobboard.buildmytalent.com"; // TODO-TO BE CHANGED
            var confirmationPath = "/Account/ConfirmEmail";
            var callbackUrl = GetConfirmationUrl(baseUrl, confirmationPath, user.Id, code);

            if (string.IsNullOrEmpty(password))
                await SendEmailVerificationRequestAsync(new JobPortal.Account.SendEmailVerificationDto { Email = user.Email, CallbackUrl = callbackUrl });
            else
                await SendEmailVerificationInJobApplicationRequestAsync(new JobPortal.Account.SendEmailVerificationDto { Email = user.Email, CallbackUrl = callbackUrl}, password);
        }

        private string GetConfirmationUrl(string baseUrl, string confirmationPath, Guid userId, string code)
        {
            return $"{baseUrl}{confirmationPath}?userId={userId}&code={code}";
        }


        private async Task AssignDefaultRoles(UserType userType, IdentityUser user)
        {
            if (userType == UserType.Organisation)
            {
                await UserManager.AddToRoleAsync(user, RoleNames.OrganisationOwnerRole);
            }
            if (userType == UserType.Candidate)
            {
                await UserManager.AddToRoleAsync(user, RoleNames.BmtApplicant);
            }
        }
    }
}
