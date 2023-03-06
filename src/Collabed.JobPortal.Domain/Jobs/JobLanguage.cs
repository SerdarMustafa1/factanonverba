using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobLanguage : Entity<int>
    {
        public Guid JobId { get; private set; }
        public int LanguageId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobLanguage()
        {
        }

        internal JobLanguage(Guid jobId, int languageId)
        {
            JobId = jobId;
            LanguageId = languageId;
        }

        public override object[] GetKeys()
        {
            return new object[] { LanguageId, JobId };
        }
    }
}