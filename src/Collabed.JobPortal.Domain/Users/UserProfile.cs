using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Users
{
    public class UserProfile : Entity<Guid>
    {
        private UserProfile()
        {
        }

        internal UserProfile(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
        public string PostCode { get; set; }
        public string CvBlobName { get; set; }
        public string CvFileName { get; set; }
        public string CvContentType { get; set; }
    }
}
