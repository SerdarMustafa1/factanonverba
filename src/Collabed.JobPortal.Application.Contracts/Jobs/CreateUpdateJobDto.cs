using System;
using System.Collections.Generic;
using System.Text;
using Collabed.JobPortal.Job;
using Collabed.JobPortal.Types;

namespace Collabed.JobPortal.Jobs
{
    public class CreateUpdateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string Skills { get; set; }
        public int? OfficeLocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? IsLocalLanguageRequired { get; set; }
        public int? LocalLanguageId { get; set; }
        public bool? OfferVisaSponsorship { get; set; }
        public float? SalaryMinimum { get; set; }
        public float? SalaryMaximum { get; set; }
        public bool? IsSalaryNegotiable { get; set; }
        public string SalaryOtherBenefits { get; set; }
        public string SupplementalPay { get; set; }
        public bool? HiringMultipleCandidates { get; set; }
        public int? PositionsAvailable { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public CurrencyType? SalaryCurrency { get; set; }
        public ContractType ContractType { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public SalaryPeriod? PaymentOption { get; set; }
        public JobLocation JobLocation { get; set; }
        public ExperienceLevel? ExperienceLevel { get; set; }
        public ICollection<int> SupportingDocuments { get; set; }
        public IEnumerable<(string, bool?)> ScreeningQuestions { get; set; }
        public string OtherDocuments { get; set; }
        public bool? IsNetZeroCompliant { get; set; }
    }
}
