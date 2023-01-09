using System;
using Collabed.JobPortal.ErrorCodes;
using Collabed.JobPortal.Job;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Collabed.JobPortal.Jobs
{
    internal class Job : AuditedAggregateRoot<Guid>
    {
        private Job()
        {
        }

        public Job(string contactEmail, string contactUrl, string reference, string title, JobType type, string duration, string startDate, string skills, string description, LocationType location, IndustryType industry, CurrencyType salaryCurrency, float salaryFrom, float salaryTo, SalaryPeriodType salaryPeriod, string salaryBenefits, string salary)
        {
            SetContactEmail(contactEmail);
            SetContactUrl(contactUrl);
            SetReference(reference);
            SetTitle(title);
            SetType(type);
            SetDuration(duration);
            SetStartDate(startDate);
            SetSkills(skills);
            SetDescription(description);
            SetLocation(location);
            SetIndustry(industry);
            SetSalaryCurrency(salaryCurrency);
            SetSalaryRange(salaryFrom, salaryTo);
            SetSalaryPeriod(salaryPeriod);
            SetSalaryBenefits(salaryBenefits);
            SetSalary(salary);
        }

        public string ContactEmail { get; private set; }
        public string ContactPhone { get; private set; }
        public string ContactUrl { get; private set; }
        public string Reference { get; private set; }
        public string Title { get; private set; }
        public JobType Type { get; private set; }
        public string Duration { get; private set; }
        public string StartDate { get; private set; }
        public string Skills { get; private set; }
        public string Description { get; private set; }
        public LocationType Location { get; private set; }
        public IndustryType Industry { get; private set; }
        public CurrencyType SalaryCurrency { get; private set; }
        public float SalaryFrom { get; private set; }
        public float SalaryTo { get; private set; }
        public SalaryPeriodType SalaryPeriod { get; private set; }
        public string SalaryBenefits { get; private set; }
        public string Salary { get; private set; }

        #region Public setters
        public Job SetContactEmail(string contactEmail)
        {
            ContactEmail = contactEmail;
            return this;
        }
        public Job SetContactPhone(string contactPhone)
        {
            ContactPhone = contactPhone;
            return this;
        }
        public Job SetContactUrl(string contactUrl)
        {
            ContactUrl = contactUrl;
            return this;
        }
        public Job SetType(JobType type)
        {
            Type = type;
            return this;
        }
        public Job SetDuration(string duration)
        {
            Duration = duration;
            return this;
        }
        public Job SetStartDate(string startDate)
        {
            StartDate = startDate;
            return this;
        }
        public Job SetSkills(string skills)
        {
            Skills = skills;
            return this;
        }
        public Job SetDescription(string description)
        {
            Description = description;
            return this;
        }
        public Job SetLocation(LocationType location)
        {
            Location = location;
            return this;
        }
        public Job SetIndustry(IndustryType industry)
        {
            Industry = industry;
            return this;
        }
        public Job SetSalaryCurrency(CurrencyType salaryCurrency)
        {
            SalaryCurrency = salaryCurrency;
            return this;
        }
        public Job SetSalaryPeriod(SalaryPeriodType salaryPeriod)
        {
            SalaryPeriod = salaryPeriod;
            return this;
        }
        public Job SetSalaryBenefits(string salaryBenefits)
        {
            SalaryBenefits = salaryBenefits;
            return this;
        }

        public Job SetSalary(string salary)
        {
            Salary = salary;
            return this;
        }
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
