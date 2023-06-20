using Collabed.JobPortal.Types;
using System;

namespace Collabed.JobPortal.Applications
{
    public class ApplicationSummaryDto
    {
        public Guid ApplicationId { get; set; }
        public string JobTitle { get; set; }
        public string JobReference { get; set; }
        public string AppReference { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public Guid UserId { get; set; }
        public string CvFileName { get; set; }
        public string CvContentType { get; set; }
        public string PortfolioLink { get; set; }
        public string CoverLetter { get; set; }
        public string CvBlobFileName { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int? Rating { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
