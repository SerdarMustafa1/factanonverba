using Collabed.JobPortal.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Collabed.JobPortal.Organisations
{
    public class Organisation : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
        public ICollection<OrganisationMember> Members { get; private set; }
        public ICollection<Jobs.Job> PostedJobs { get; private set; }

        private Organisation()
        {
        }

        public Organisation(Guid id, string name, string emailAddress) : base(id)
        {
            SetName(name);
            SetEmail(emailAddress);
            Members = new Collection<OrganisationMember>();
            PostedJobs = new Collection<Jobs.Job>();
        }

        public Organisation SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            return this;
        }

        public Organisation SetEmail(string email)
        {
            Check.NotNullOrWhiteSpace(email, nameof(email));
            if (!RegexUtilities.IsValidEmail(email))
            {
                throw new BusinessException($"Email: {email} is invalid.");
            }
            EmailAddress = email;

            return this;
        }

        public void AddMember(Guid userId)
        {
            Check.NotNull(userId, nameof(userId));

            if (IsInOrganisation(userId))
            {
                return;
            }

            Members.Add(new OrganisationMember(Id, userId));
        }

        public void RemoveMember(Guid userId)
        {
            Check.NotNull(userId, nameof(userId));

            if (!IsInOrganisation(userId))
            {
                return;
            }

            Members.RemoveAll(x => x.UserId == userId);
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

        private bool IsInOrganisation(Guid userId)
        {
            return Members.Any(x => x.UserId == userId);
        }
    }
}
