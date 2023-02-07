using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobApplicant : Entity
    {
        public Guid UserId { get; private set; }
        public Guid JobId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobApplicant()
        {
        }

        internal JobApplicant(Guid userId, Guid jobId)
        {
            UserId = userId;
            JobId = jobId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, JobId };
        }
    }
}
