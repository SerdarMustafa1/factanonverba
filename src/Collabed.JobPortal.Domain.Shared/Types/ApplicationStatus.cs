using Collabed.JobPortal.Attributes;

namespace Collabed.JobPortal.Types
{
    public enum ApplicationStatus
    {
        [Order(0)]
        New,
        [Order(1)]
        Interview,
        [Order(2)]
        Review,
        [Order(3)]
        Final,
        [Order(5)]
        Rejected,
        [Order(4)]
        Hired
    }
}
