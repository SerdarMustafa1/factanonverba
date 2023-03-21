using Collabed.JobPortal.Types;
using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobApplicant : Entity
    {
        public Guid UserId { get; private set; }
        public Guid JobId { get; private set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; private set; }
        public DateTime? InterviewDate { get; set; }
        public bool StatusChangePublished { get; set; }
        public bool NotificationSent { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobApplicant()
        {
        }

        internal JobApplicant(Guid userId, Guid jobId)
        {
            UserId = userId;
            JobId = jobId;
            ApplicationStatus = ApplicationStatus.New;
            ApplicationDate = DateTime.Now;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, JobId };
        }
    }
}
