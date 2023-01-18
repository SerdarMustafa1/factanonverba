using Collabed.JobPortal.Candidates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Clients
{
    public class Client : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public ICollection<Jobs.Job> PostedJobs { get; set; }
        private Client()
        {
        }

        public Client(Guid id, Guid userId) : base(id)
        {
            SetUserId(userId);
            PostedJobs = new Collection<Jobs.Job>();
        }

        public Client SetUserId(Guid userId)
        {
            UserId = userId;
            return this;
        }
        public void AddJob(Jobs.Job job)
        {
            Check.NotNull(job, nameof(job));

            if (IsInJob(job.Id))
            {
                return;
            }

            PostedJobs.Add(job);
        }

        public void RemoveJob(Jobs.Job job)
        {
            Check.NotNull(job, nameof(job));

            if (!IsInJob(job.Id))
            {
                return;
            }

            PostedJobs.RemoveAll(x => x.Id == job.Id);
        }

        private bool IsInJob(Guid jobId)
        {
            return PostedJobs.Any(x => x.Id == jobId);
        }
    }
}
