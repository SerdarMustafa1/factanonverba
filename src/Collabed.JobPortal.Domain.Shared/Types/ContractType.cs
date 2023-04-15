using System.Runtime.Serialization;

namespace Collabed.JobPortal.Types
{
    public enum ContractType
    {
        Permanent,
        Temporary,
        [IgnoreDataMember]
        Internship,
        [IgnoreDataMember]
        Apprenticeship,
        Contract
    }
}
