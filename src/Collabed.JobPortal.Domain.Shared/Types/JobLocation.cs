using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Collabed.JobPortal.Types
{
    public enum JobLocation
    {
        [Display(Name = "Office/Site Based")]
        InOffice,
        Hybrid,
        Remote,
        [IgnoreDataMember]
        Unknown
    }
}
