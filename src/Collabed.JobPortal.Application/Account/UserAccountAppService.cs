using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountAppService), typeof(AccountAppService), typeof(UserAccountAppService))]
    public class UserAccountAppService : AccountAppService
    {

        public UserAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
        }
        public override async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            var userDto = await base.RegisterAsync(input);

            return userDto;
        }
    }
}
