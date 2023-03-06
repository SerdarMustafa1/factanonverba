using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobSupportingDocument : Entity<int>
    {
        public Guid JobId { get; private set; }
        public int SupportingDocumentId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobSupportingDocument()
        {
        }

        internal JobSupportingDocument(Guid jobId, int supportingDocumentId)
        {
            JobId = jobId;
            SupportingDocumentId = supportingDocumentId;
        }

        public override object[] GetKeys()
        {
            return new object[] { SupportingDocumentId, JobId };
        }
    }
}