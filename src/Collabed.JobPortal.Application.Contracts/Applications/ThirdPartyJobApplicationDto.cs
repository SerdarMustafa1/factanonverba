using Collabed.JobPortal.Jobs;
using System.Collections.Generic;

namespace Collabed.JobPortal.Applications
{
    public class ThirdPartyJobApplicationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PostCode { get; set; }
        public string CvBlobName { get; set; }
        public string ContentType { get; set; }
        public string CompanyName { get; set; }
        public string JobPosition { get; set; }
        public string CvFileName { get; set; }
        public string CoverLetter { get; set; }
        public string PortfolioLink { get; set; }
        public IEnumerable<ScreeningQuestionDto> ScreeningQuestions { get; set; }
    }
}
