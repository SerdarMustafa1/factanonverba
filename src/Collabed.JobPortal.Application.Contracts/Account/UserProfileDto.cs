using System;

namespace Collabed.JobPortal.Account
{
    public class UserProfileDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public string CvBlobName { get; set; }
        public string CvFileName { get; set; }
        public string CvContentType { get; set; }
    }
}
