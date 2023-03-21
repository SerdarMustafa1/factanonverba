using Collabed.JobPortal.Types;
using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;

namespace Collabed.JobPortal.Jobs
{
    public class JobWithApplications : IHasCreationTime
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public string Title { get; set; }
        public Guid? OrganisationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public JobStatus Status { get; set; }
        public IEnumerable<JobApplicant> Applicants { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
