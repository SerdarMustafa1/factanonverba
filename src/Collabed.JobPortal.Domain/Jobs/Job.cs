using Collabed.JobPortal.ErrorCodes;
using Collabed.JobPortal.Job;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Collabed.JobPortal.Jobs
{
    public class Job : AuditedAggregateRoot<Guid>
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
        public string Industry { get; set; }
        public CurrencyType? SalaryCurrency { get; set; }
        public float? SalaryFrom { get; set; }
        public float? SalaryTo { get; set; }
        public SalaryPeriodType? SalaryPeriod { get; set; }
        public string SalaryBenefits { get; set; }
        public string Salary { get; set; }
        public JobOrigin JobOrigin { get; set; }
        public JobStatus Status { get; set; }
        public Guid? OrganisationId { get; set; }
        public int? LocationId { get; set; }
        public ICollection<JobApplicant> Applicants { get; set; }
        public ICollection<JobCategory> Categories { get; set; }
        public ICollection<JobLanguage> Languages { get; set; }
        public ICollection<JobSchedule> Schedules { get; set; }
        public ICollection<JobSupplementalPay> SupplementalPays { get; set; }
        public ICollection<JobSupportingDocument> SupportingDocuments { get; set; }

        private Job()
        {
        }

        internal Job(string title, Guid? organisationId)
        {
            SetTitle(title);
            if (organisationId != null)
            {
                OrganisationId = organisationId;
            }
            Reference = GenerateJobReference();
        }

        internal Job(string reference)
        {
            SetReference(reference);
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

        private string GenerateJobReference()
        {
            var res = new Random();

            // String that contain both alphabets and numbers
            var str = "abcdefghijklmnopqrstuvwxyz0123456789";
            int size = 8;

            // Initializing the empty string
            string randomString = "";

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly
                int x = res.Next(str.Length);

                // Appending the character at the 
                // index to the random alphanumeric string.
                randomString += str[x];
            }

            return randomString;
        }
    }
}
