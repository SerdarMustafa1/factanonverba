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

        public async Task<Job> UpdateAsync(Guid userId, Job job, string title, string description)
        {
            job.Title = title;
            job.Description = description;

            return job;
        }

        public IEnumerable<ScreeningQuestion> CreateScreeningQuestions(IEnumerable<(string, bool)> screeningQuestions, Guid jobId)
        {
            var screeningQuestionsCollection = new List<ScreeningQuestion>();

            foreach (var (text, isAutoReject) in screeningQuestions)
            {
                screeningQuestionsCollection.Add(new ScreeningQuestion(GuidGenerator.Create(), jobId, text, isAutoReject));
            }

            return screeningQuestionsCollection;
        }

        // Add any other domain service methods
        // Note:    Do not create domain service methods simply to change the
        //          entity properties without any business logic.
    }
}
