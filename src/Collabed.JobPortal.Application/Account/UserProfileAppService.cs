using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Collabed.JobPortal.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(UserProfileAppService), typeof(IUserProfileAppService))]
    public class UserProfileAppService : IdentityAppServiceBase, IUserProfileAppService
    {
        private IdentityUserManager UserManager { get; }
        private IOptions<IdentityOptions> IdentityOptions { get; }

        public UserProfileAppService(IdentityUserManager userManager,
            IOptions<IdentityOptions> identityOptions)
        {
            UserManager = userManager;
            IdentityOptions = identityOptions;
        }

        public async Task<ProfileDto> GetAsync()
        {
            var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
            var profileDto = ObjectMapper.Map<IdentityUser, ProfileDto>(currentUser);
            profileDto.UserType = (UserType)Enum.Parse(typeof(UserType), currentUser.GetUserType(), true);

            return profileDto;
        }

        public async Task<ProfileDto> UpdateAsync(UpdateBaseProfileDto input)
        {
            await IdentityOptions.SetAsync();

            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

            //user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            if (!string.Equals(user.UserName, input.UserName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled))
                {
                    (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
                }
            }

            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled))
                {
                    (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
                }
            }
            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }
            user.SetEmailConfirmed(true);
            user.Name = input.Name;
            user.Surname = input.Surname;

            input.MapExtraPropertiesTo(user);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, ProfileDto>(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordInput input)
        {
            await IdentityOptions.SetAsync();

            var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

            if (currentUser.IsExternal)
            {
                throw new BusinessException(code: IdentityErrorCodes.ExternalUserPasswordChange);
            }

            if (currentUser.PasswordHash == null)
            {
                (await UserManager.AddPasswordAsync(currentUser, input.Password)).CheckErrors();

                return;
            }

            (await UserManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.Password)).CheckErrors();
        }
    }
}
