using System.ComponentModel.DataAnnotations;

namespace Collabed.JobPortal.Types
{
    public enum JobLocation
    {
        [Display(Name = "In-office")]
        InOffice,
        Hybrid,
        Remote,
        Unknown
    }
}
