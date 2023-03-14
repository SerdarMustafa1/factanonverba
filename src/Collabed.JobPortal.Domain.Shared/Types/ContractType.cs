using System.Runtime.Serialization;

namespace Collabed.JobPortal.Types
{
    public enum ContractType
    {
        Permanent,
        Temporary,
        Internship,
        Apprenticeship,
        [IgnoreDataMember]
        Contract
    }
}
