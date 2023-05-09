using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Collabed.JobPortal.Types
{
    public enum EmploymentType
    {
        [Display(Name = "Full-time")]
        Fulltime,
        [Display(Name = "Part-time")]
        Parttime,
        [Display(Name = "Graduate Scheme")]
        [IgnoreDataMember]
        GraduateScheme,
        [Display(Name = "Work Placement")]
        [IgnoreDataMember]
        WorkPlacement,
        [IgnoreDataMember]
        Unknown
    }
}
