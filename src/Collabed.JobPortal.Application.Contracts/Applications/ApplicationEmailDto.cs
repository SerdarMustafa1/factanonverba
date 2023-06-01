namespace Collabed.JobPortal.Applications
{
    public class ApplicationEmailDto : ApplicationDto
    {
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string JobPosition { get; set; }
        public string CvBlobName { get; set; }
    }
}
