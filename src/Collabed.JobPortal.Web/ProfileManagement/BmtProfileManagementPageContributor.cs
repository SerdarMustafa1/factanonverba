using Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Password;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Collabed.JobPortal.Web.ProfileManagement
{
    public class BmtProfileManagementPageContributor : IProfileManagementPageContributor
    {
        public async Task ConfigureAsync(ProfileManagementPageCreationContext context)
        {
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AccountResource>>();

            if (await IsPasswordChangeEnabled(context))
            {
                context.Groups.Add(
                    new ProfileManagementPageGroup(
                        "Collabed.JobPortal.Account.Password",
                        l["ProfileTab:Password"],
                        typeof(AccountProfilePasswordManagementGroupViewComponent)
                    )
                );
            }

            context.Groups.Add(
                new ProfileManagementPageGroup(
                    "Collabed.JobPortal.Account.PersonalInfo",
                    l["ProfileTab:PersonalInfo"],
                    typeof(AccountProfilePersonalInfoManagementGroupViewComponent)
                )
            );
        }

        protected virtual async Task<bool> IsPasswordChangeEnabled(ProfileManagementPageCreationContext context)
        {
            var userManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

            var user = await userManager.GetByIdAsync(currentUser.GetId());

            return !user.IsExternal;
        }
    }
}
