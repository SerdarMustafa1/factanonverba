namespace Collabed.JobPortal.Jobs
{
    public class JobSummaryDto
    {
        public string Reference { get; set; }
        public string Title { get; set; }
        public int ApplicationsCount { get; set; }
        public int InProcessCount { get; set; }
        public int HiredCount { get; set; }
        public string DaysLeft { get; set; }
        public string Status { get; set; }
    }
}
