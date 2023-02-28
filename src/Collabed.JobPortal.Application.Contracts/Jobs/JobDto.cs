using Collabed.JobPortal.Job;
using System;

namespace Collabed.JobPortal.Jobs
{
    public class JobDto
    {
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactUrl { get; set; }
        public string ContactName { get; set; }
        public int DaysToAdvertise { get; set; }
        public string ApplicationEmail { get; set; }
        public string ApplicationUrl { get; set; }
        public string Reference { get; set; }
        public string Title { get; set; }
        public JobType? Type { get; set; }
        public string Duration { get; set; }
        public string StartDate { get; set; }
        public string Skills { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }
        public CurrencyType? SalaryCurrency { get; set; }
        public float? SalaryFrom { get; set; }
        public float? SalaryTo { get; set; }
        public SalaryPeriodType? SalaryPeriod { get; set; }
        public string SalaryBenefits { get; set; }
        public string Salary { get; set; }
        public JobOrigin JobOrigin { get; set; }
        public Guid? OrganisationId { get; set; }
        //public ICollection<JobApplicant> Applicants { get; set; }

    }
}
