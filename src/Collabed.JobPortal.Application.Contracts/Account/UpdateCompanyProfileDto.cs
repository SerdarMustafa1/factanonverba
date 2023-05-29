using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Collabed.JobPortal.Account
{
    public class UpdateCompanyProfileDto : UpdateBaseProfileDto
    {
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string LogoFileName { get; set; }
        public string LogoContentType { get; set; }

        public string LogoBlobName { get; set; }
        public Stream FileStream { get; set; }
    }
}
