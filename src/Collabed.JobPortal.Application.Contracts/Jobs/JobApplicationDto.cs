using Collabed.JobPortal.Types;
using System;

namespace Collabed.JobPortal.Jobs
{
    public class JobApplicationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid JobApplicantId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid JobId { get; private set; }
        public string CvBlobName { get; set; }
        public string CvFileName { get; set; }
        public string CvContentType { get; set; }
        public string CoverLetter { get; set; }
        public string Portfolio { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; private set; }
        public DateTime? InterviewDate { get; set; }
        public bool StatusChangePublished { get; set; }
        public bool NotificationSent { get; set; }
    }
}
