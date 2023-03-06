using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobSupplementalPay : Entity<int>
    {
        public Guid JobId { get; private set; }
        public int SupplementalPayId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobSupplementalPay()
        {
        }

        internal JobSupplementalPay(Guid jobId, int supplementalPayId)
        {
            JobId = jobId;
            SupplementalPayId = supplementalPayId;
        }

        public override object[] GetKeys()
        {
            return new object[] { SupplementalPayId, JobId };
        }
    }
}