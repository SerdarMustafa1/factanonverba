using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountAppService), typeof(AccountAppService), typeof(IBmtAccountAppService))]
    public class BmtAccountAppService : AccountAppService, IBmtAccountAppService
    {
        private readonly IBmtAccountEmailer _accountEmailer;

        public BmtAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IBmtAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _accountEmailer = accountEmailer;
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
    }
}
