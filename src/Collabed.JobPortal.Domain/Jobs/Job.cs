using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.ErrorCodes;
using Collabed.JobPortal.Job;
using Collabed.JobPortal.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Collabed.JobPortal.Jobs
{
    public class Job : AuditedAggregateRoot<Guid>
    {
        public string Reference { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string Skills { get; set; }
        public int? OfficeLocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? IsLocalLanguageRequired { get; set; }
        public int? LocalLanguageId { get; set; }
        public bool? OfferVisaSponsorship { get; set; }
        public float? SalaryFrom { get; set; }
        public float? SalaryTo { get; set; }
        public decimal? MinSalaryConverted { get; set; }
        public decimal? MaxSalaryConverted { get; set; }
        public bool? IsSalaryNegotiable { get; set; }
        public string SalaryBenefits { get; set; }
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
        public ICollection<JobSchedule> Schedules { get; set; }
        public string SupplementalPay { get; set; }
        public ICollection<JobSupportingDocument> SupportingDocuments { get; set; }
        public IEnumerable<ScreeningQuestion> ScreeningQuestions { get; set; }
        public string OtherDocuments { get; set; }
        public bool IsAcceptingApplications { get; set; } = true;
        public JobOrigin JobOrigin { get; set; }
        public JobStatus Status { get; set; }
        public Guid? OrganisationId { get; set; }
        public bool? IsNetZeroCompliant { get; set; }
        public IEnumerable<JobApplicant> Applicants { get; set; }

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
        public string CompanyName { get; set; }
        public string ContactUrl { get; set; }

        private Job()
        {
        }

        internal Job(Guid id, Guid? organisationId) : base(id)
        {
            if (organisationId != null)
            {
                OrganisationId = organisationId;
            }
            Reference = GenerateJobReference();
            Schedules = new Collection<JobSchedule>();
            SupportingDocuments = new Collection<JobSupportingDocument>();
        }

        // This constructor is used for external jobs feed
        internal Job(Guid id, string reference) : base(id)
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
        public Job SetSchedules(IEnumerable<int> scheduleIds)
        {
            foreach (var schedule in scheduleIds)
            {
                Schedules.Add(new JobSchedule(Id, schedule));
            }

            return this;
        }
        public Job SetSupportingDocs(IEnumerable<int> supportingDocumentIds)
        {
            foreach (var item in supportingDocumentIds)
            {
                SupportingDocuments.Add(new JobSupportingDocument(Id, item));
            }

            return this;
        }
        #endregion

        private string GenerateJobReference()
        {
            var res = new Random();

            // String that contain both alphabets and numbers
            var str = "abcdefghijklmnopqrstuvwxyz0123456789";
            int size = 10;

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
