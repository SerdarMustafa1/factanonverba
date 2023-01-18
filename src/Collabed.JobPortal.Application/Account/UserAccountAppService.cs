using Collabed.JobPortal.Candidates;
using Collabed.JobPortal.Clients;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace Collabed.JobPortal.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountAppService), typeof(AccountAppService), typeof(UserAccountAppService))]
    public class UserAccountAppService : AccountAppService
    {
        private readonly IRepository<Client, Guid> _clientRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;

        public UserAccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions,
            IRepository<Client, Guid> clientRepository,
            IRepository<Candidate, Guid> candidateRepository) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _clientRepository = clientRepository;
            _candidateRepository = candidateRepository;
        }
        public override async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            input.SetUserType(UserType.Client);
            var userDto = await base.RegisterAsync(input);

            // TODO: user type will come from register flow
            var userType = UserType.Client;
            if (userType == UserType.Client)
            {
                var client = new Client(GuidGenerator.Create(), userDto.Id);
                await _clientRepository.InsertAsync(client);
            }

            if (userType == UserType.Candidate)
            {
                var candidate = new Candidate(GuidGenerator.Create(), userDto.Id);
                await _candidateRepository.InsertAsync(candidate);
            }

            return userDto;
        }
    }
}
