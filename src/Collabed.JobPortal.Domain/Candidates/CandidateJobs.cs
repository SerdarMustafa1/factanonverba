using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Candidates
{
    public class CandidateJobs : Entity
    {
        public Guid CandidateId { get; private set; }
        public Guid JobId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private CandidateJobs()
        {
        }

        internal CandidateJobs(Guid candidateId, Guid jobId)
        {
            CandidateId = candidateId;
            JobId = jobId;
        }

        public override object[] GetKeys()
        {
            return new object[] { CandidateId, JobId };
        }
    }
}
