using System.Runtime.Serialization;

namespace Collabed.JobPortal.Types
{
    public enum SalaryPeriod
    {
        Hourly,
        [IgnoreDataMember]
        Daily,
        [IgnoreDataMember]
        Weekly,
        Monthly,
        Yearly
    }
}
