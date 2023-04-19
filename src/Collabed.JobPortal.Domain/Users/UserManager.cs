using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Users
{
    public class UserManager : DomainService
    {
        private readonly IRepository<IdentityUser, Guid> _userRepository;

        public UserManager(IRepository<IdentityUser, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public UserProfile CreateUserProfile(Guid userId)
        {
            return new UserProfile(userId);
        }
    }
}
