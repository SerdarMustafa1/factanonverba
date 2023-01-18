using Collabed.JobPortal.Candidates;
using Collabed.JobPortal.Clients;
using Collabed.JobPortal.User;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IRepository<Client, Guid> _clientRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;

        public UserAppService(IRepository<Client, Guid> clientRepository, IRepository<Candidate, Guid> candidateRepository)
        {
            _clientRepository = clientRepository;
            _candidateRepository = candidateRepository;
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public async Task CreateAsync(Guid identityUserId, UserType userType)
        {
            if (userType == UserType.Client)
            {
                var client = new Client(GuidGenerator.Create(), identityUserId);
                await _clientRepository.InsertAsync(client);
            }

            if (userType == UserType.Candidate)
            {
                var candidate = new Candidate(GuidGenerator.Create(), identityUserId);
                await _candidateRepository.InsertAsync(candidate);
            }
        }
    }
}
