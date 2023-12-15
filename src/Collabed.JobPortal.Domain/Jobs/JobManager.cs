using Collabed.JobPortal.DropDowns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Collabed.JobPortal.Jobs
{
    public class JobManager : DomainService
    {
        private readonly IRepository<Job, Guid> _jobRepository;

        public JobManager(IRepository<Job, Guid> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Job> CreateAsync(Guid organisationId)
        {
            return new Job(GuidGenerator.Create(), organisationId);
        }

        public Job CreateExternal(string reference)
        {
            return new Job(GuidGenerator.Create(), reference);
        }

        public async Task<Job> UpdateAsync(Job job, CreateUpdateJobDto input)
        {
            job.Title = input.Title;
            job.Description = input.Description;
            job.SubDescription = input.SubDescription;
            job.Skills = input.Skills;
            job.OfficeLocationId = input.OfficeLocationId;
            job.StartDate = input.StartDate;
            job.IsLocalLanguageRequired = input.IsLocalLanguageRequired;
            job.LocalLanguageId = input.LocalLanguageId;
            job.OfferVisaSponsorship = input.OfferVisaSponsorship;
            job.SalaryFrom= input.SalaryMinimum;
            job.SalaryTo = input.SalaryMaximum;
            job.IsSalaryNegotiable = input.IsSalaryNegotiable;
            job.SalaryBenefits = input.SalaryOtherBenefits;
            job.PositionsAvailable = input.PositionsAvailable;
            job.SalaryCurrency = input.SalaryCurrency;
            job.Type = input.ContractType;
            job.EmploymentType = input.EmploymentType;
            job.SalaryPeriod = input.PaymentOption;
            job.JobLocation = input.JobLocation;
            job.ExperienceLevel = input.ExperienceLevel;
            job.SupplementalPay = input.SupplementalPay;

            return job;
        }

        public IEnumerable<ScreeningQuestion> CreateScreeningQuestions(IEnumerable<(string, bool?)> screeningQuestions, Guid jobId)
        {
            var screeningQuestionsCollection = new List<ScreeningQuestion>();

            foreach (var (text, autoRejectAnswer) in screeningQuestions)
            {
                screeningQuestionsCollection.Add(new ScreeningQuestion(GuidGenerator.Create(), jobId, text, autoRejectAnswer));
            }

            return screeningQuestionsCollection;
        }

        /// <summary>
        /// Min and max salary needs to be converted to yearly rate to make the search by salary easier
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public void ConvertSalaryRates(Job job)
        {
            const decimal hoursInWeek = 37.5M;
            const decimal hoursInDay = 7.5M;
            const decimal workingDaysInMonth = 21;
            const decimal workingDaysInYear = 251;
            const decimal weeksInYear = 50.2M;
            const decimal monthsInYear = 12;


            if (!job.SalaryFrom.HasValue || !job.SalaryTo.HasValue)
                return;

            var salaryFrom = (decimal)job.SalaryFrom;
            var salaryTo = (decimal)job.SalaryTo;

            switch (job.SalaryPeriod)
            {
                case Types.SalaryPeriod.Hourly:
                    job.MinSalaryConverted = salaryFrom * hoursInDay * workingDaysInYear;
                    job.MaxSalaryConverted = salaryTo * hoursInDay * workingDaysInYear;
                    break;
                case Types.SalaryPeriod.Daily:
                    job.MinSalaryConverted = salaryFrom * workingDaysInYear;
                    job.MaxSalaryConverted = salaryTo * workingDaysInYear;
                    break;
                case Types.SalaryPeriod.Weekly:
                    job.MinSalaryConverted = salaryFrom * weeksInYear;
                    job.MaxSalaryConverted = salaryTo * weeksInYear;
                    break;
                case Types.SalaryPeriod.Monthly:
                    job.MinSalaryConverted = salaryFrom * monthsInYear;
                    job.MaxSalaryConverted = salaryTo * monthsInYear;
                    break;
                case Types.SalaryPeriod.Annually:
                    job.MinSalaryConverted = salaryFrom;
                    job.MaxSalaryConverted = salaryTo;
                    break;
                default:
                    break;
            }

            return;
        }

        // Add any other domain service methods
        // Note:    Do not create domain service methods simply to change the
        //          entity properties without any business logic.
    }
}
