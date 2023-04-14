using Collabed.JobPortal.Job;
using Collabed.JobPortal.Types;
using System;
using Volo.Abp.Auditing;

namespace Collabed.JobPortal.Jobs
{
    public class JobWithDetails : IHasCreationTime
    {
        public string Reference { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string Skills { get; set; }
        public string OfficeLocation { get; set; }
        public int? OfficeLocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? IsLocalLanguageRequired { get; set; }
        public string LocalLanguage { get; set; }
        public int? LocalLanguageId { get; set; }
        public bool? OfferVisaSponsorship { get; set; }
        public float? SalaryFrom { get; set; }
        public float? SalaryTo { get; set; }
        public bool? IsSalaryNegotiable { get; set; }
        public string SalaryBenefits { get; set; }
        public bool IsSalaryEstimated { get; set; }
        public bool? HiringMultipleCandidates { get; set; }
        public int? PositionsAvailable { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public CurrencyType? SalaryCurrency { get; set; }
        public ContractType? Type { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        public SalaryPeriod? SalaryPeriod { get; set; }
        public JobLocation? JobLocation { get; set; }
        public ExperienceLevel? ExperienceLevel { get; set; }
        public int CategoryId { get; set; }
        public string SupplementalPay { get; set; }
        public string OtherDocuments { get; set; }
        public bool IsAcceptingApplications { get; set; } = true;
        public JobOrigin JobOrigin { get; set; }
        public JobStatus Status { get; set; }
        public string OrganisationName { get; set; }
        public Guid? OrganisationId { get; set; }
        public bool? IsNetZeroCompliant { get; set; }

        // Broadbean/Idibu specific fields
        public string StartDateText { get; set; }
        public string ApplicationEmail { get; set; }
        public string ApplicationUrl { get; set; }
        public string Duration { get; set; }
        public string Industry { get; set; }
        public string Salary { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactUrl { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
