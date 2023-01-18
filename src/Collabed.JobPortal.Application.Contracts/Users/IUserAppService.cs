using Collabed.JobPortal.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Users
{
    public interface IUserAppService
    {
        Task CreateAsync(Guid identityUserId, UserType userType);
    }
}
