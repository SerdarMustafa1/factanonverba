using Collabed.JobPortal.User;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Collabed.JobPortal.Account
{
    public class ProfileDto : ExtensibleObject, IHasConcurrencyStamp
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }

        public bool IsExternal { get; set; }

        public bool HasPassword { get; set; }

        public string ConcurrencyStamp { get; set; }
        public UserType UserType { get; set; }
    }
}
