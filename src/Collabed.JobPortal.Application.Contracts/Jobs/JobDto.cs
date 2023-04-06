﻿using Collabed.JobPortal.Types;
using System;

namespace Collabed.JobPortal.Jobs
{
    public class JobDto
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
        //public ContractType? Type { get; set; }
        public string ContractType { get; set; }
        //public EmploymentType? EmploymentType { get; set; }
        public string EmploymentType { get; set; }
        //public SalaryPeriod? SalaryPeriod { get; set; }
        public string SalaryPeriod { get; set; }
        //public JobLocation? JobLocation { get; set; }
        public string JobLocation { get; set; }
        //public ExperienceLevel? ExperienceLevel { get; set; }
        public string ExperienceLevel { get; set; }
        public bool? IsNetZeroCompliant { get; set; }
        public bool? IsSalaryNegotiable { get; set; }
        public int CategoryId { get; set; }
        public string SupplementalPay { get; set; }
        public JobOrigin JobOrigin { get; set; }
        public JobStatus Status { get; set; }
        //public Guid? OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
