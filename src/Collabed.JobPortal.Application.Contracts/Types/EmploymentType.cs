using System.ComponentModel.DataAnnotations;

namespace Collabed.JobPortal.Types
{
    public enum EmploymentType
    {
        [Display(Name = "Full-time")]
        Fulltime,
        [Display(Name = "Part-time")]
        Parttime
    }
}
