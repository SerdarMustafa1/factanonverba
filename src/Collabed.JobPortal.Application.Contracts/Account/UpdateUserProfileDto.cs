using Microsoft.AspNetCore.Http;

namespace Collabed.JobPortal.Account
{
    public class UpdateUserProfileDto : UpdateBaseProfileDto
    {
        public string CvFileName { get; set; }
        public IFormFile NewCv { get; set; }
        public string PostCode { get; set; }

    }
}
