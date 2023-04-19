using Collabed.JobPortal.Jobs;
using System;
using System.Collections.Generic;

namespace Collabed.JobPortal.Applications
{
    public class ApplicationDto
    {
        public string JobReference { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public IEnumerable<ScreeningQuestionDto> ScreeningQuestions { get; set; }
        public bool IsNewCvAttached { get; set; }
        public string CvFileName { get; set; }
        public string CvContentType { get; set; }
        public byte[] CvContent { get; set; }
        public string PortfolioLink { get; set; }
        public string CoverLetter { get; set; }
    }
}
