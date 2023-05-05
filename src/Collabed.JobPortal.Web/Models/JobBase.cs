using Collabed.JobPortal.Types;
using System;

namespace Collabed.JobPortal.Web.Models
{
    public class JobBase
    {
        public string Reference { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string Skills { get; set; }
        public string OfficeLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateText { get; set; }
        public string LocalLanguage { get; set; }
        public bool? OfferVisaSponsorship { get; set; }
        public float? SalaryMinimum { get; set; }
        public float? SalaryMaximum { get; set; }
        public string SalaryBenefits { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string ContractType { get; set; }
        public string EmploymentType { get; set; }
        public string SalaryPeriod { get; set; }
        public string JobLocation { get; set; }
        public string ExperienceLevel { get; set; }
        public bool? IsNetZeroCompliant { get; set; }
        public bool? IsSalaryNegotiable { get; set; }
        public int CategoryId { get; set; }
        public bool IsSalaryEstimated { get; set; }
        public string SupplementalPay { get; set; }
        public JobOrigin JobOrigin { get; set; }
        public JobStatus Status { get; set; }
        public string OrganisationName { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ApplicationUrl { get; set; }
    }
}
