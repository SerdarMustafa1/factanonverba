using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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

        public BmtAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IBmtAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions,
            IRepository<UserProfile> userProfileRepository) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _accountEmailer = accountEmailer;
            _userProfileRepository = userProfileRepository;
        }

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
            };

            var userProfile = await _userProfileRepository.FindAsync(x => x.UserId == CurrentUser.Id);
            if (userProfile == null)
            {
                return userProfileDto;
            }

            userProfileDto.CvBlobName = userProfile.CvBlobName;
            userProfileDto.CvFileName= userProfile.CvFileName;
            userProfileDto.CvContentType = userProfile.CvContentType;

            return userProfileDto;
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
    }
}
