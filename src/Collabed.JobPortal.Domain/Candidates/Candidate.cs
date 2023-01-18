using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collabed.JobPortal.Jobs;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Candidates
{
    public class Candidate : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public ICollection<CandidateJobs> AppliedJobs { get; private set; }

        private Candidate()
        {
        }

        public Candidate(Guid id, Guid userId) : base(id)
        {
            SetUserId(userId);
            AppliedJobs = new Collection<CandidateJobs>();
        }


        public Candidate SetUserId(Guid userId)
        {
            UserId = userId;
            return this;
        }

        public void AddJob(Guid jobId)
        {
            Check.NotNull(jobId, nameof(jobId));

            if (IsInJob(jobId))
            {
                return;
            }

            AppliedJobs.Add(new CandidateJobs(Id, jobId));
        }

        public void RemoveJob(Guid jobId)
        {
            Check.NotNull(jobId, nameof(jobId));

            if (!IsInJob(jobId))
            {
                return;
            }

            AppliedJobs.RemoveAll(x => x.JobId == jobId);
        }

        private bool IsInJob(Guid jobId)
        {
            return AppliedJobs.Any(x => x.JobId == jobId);
        }
    }
}
