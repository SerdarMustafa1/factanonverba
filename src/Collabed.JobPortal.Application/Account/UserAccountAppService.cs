using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountAppService), typeof(AccountAppService), typeof(UserAccountAppService))]
    public class UserAccountAppService : AccountAppService
    {
        private readonly IRepository<Organisation, Guid> _organisationRepository;

        public UserAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions,
            IRepository<Organisation, Guid> organisationRepository) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _organisationRepository = organisationRepository;
        }
        public override async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            var userType = input.GetUserType();
            var userDto = await base.RegisterAsync(input);

            if (userType == UserType.Organisation)
            {
                // TODO: Post-MVP - Update organisation creation flow
                var organisation = new Organisation(GuidGenerator.Create(), input.GetOrganisationName(), input.EmailAddress);
                await _organisationRepository.InsertAsync(organisation);

                // For now user creating an organisation is automatically its member
                organisation.AddMember(userDto.Id);

                // TODO: Assign admin role to the user that created an organisation
            }

            return userDto;
        }
    }
}
