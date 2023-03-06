using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobCategory : Entity<int>
    {
        public Guid JobId { get; private set; }
        public int CategoryId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobCategory()
        {
        }

        internal JobCategory(Guid jobId, int categoryId)
        {
            JobId = jobId;
            CategoryId = categoryId;
        }

        public override object[] GetKeys()
        {
            return new object[] { CategoryId, JobId };
        }
    }
}
