using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobSchedule : Entity<int>
    {
        public Guid JobId { get; private set; }
        public int ScheduleId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobSchedule()
        {
        }

        internal JobSchedule(Guid jobId, int scheduleId)
        {
            JobId = jobId;
            ScheduleId = scheduleId;
        }

        public override object[] GetKeys()
        {
            return new object[] { ScheduleId, JobId };
        }
    }
}