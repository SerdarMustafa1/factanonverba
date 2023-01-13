using System;
using System.Collections;
using System.Collections.Generic;
using Collabed.JobPortal.Clients;
using Collabed.JobPortal.ErrorCodes;
using Collabed.JobPortal.Job;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Collabed.JobPortal.Jobs
{
    public class Job : AuditedAggregateRoot<Guid>
    {
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactUrl { get; set; }
        public string Reference { get; set; }
        public string Title { get; set; }
        public JobType? Type { get; set; }
        public string Duration { get; set; }
        public string StartDate { get; set; }
        public string Skills { get; set; }
        public string Description { get; set; }
        public LocationType? Location { get; set; }
        public IndustryType? Industry { get; set; }
        public CurrencyType? SalaryCurrency { get; set; }
        public float? SalaryFrom { get; set; }
        public float? SalaryTo { get; set; }
        public SalaryPeriodType? SalaryPeriod { get; set; }
        public string SalaryBenefits { get; set; }
        public string Salary { get; set; }
        public bool IsExternal { get; set; }
        public Client Client { get; set; }

        private Job()
        {
        }

        internal Job(string title, Client client)
        {
            SetTitle(title);
            Client = client;
        }

        #region Public setters
        public Job SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), JobConsts.MaxTitleLength);
            return this;
        }
        public Job SetReference(string reference)
        {
            Reference = Check.NotNullOrWhiteSpace(reference, nameof(reference), JobConsts.MaxReferenceLength);
            return this;
        }
        public Job SetSalaryRange(float salaryFrom, float salaryTo)
        {
            if (salaryFrom > salaryTo)
            {
                throw new BusinessException(JobErrorCodes.SalaryFromCantBeGreaterThanSalaryTo);
            }

            SalaryFrom = salaryFrom; 
            SalaryTo = salaryTo;

            return this;
        }
        #endregion
    }
}
