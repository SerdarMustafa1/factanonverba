using System;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Account
{
    public class RegisterUserDto : IdentityUserDto
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
