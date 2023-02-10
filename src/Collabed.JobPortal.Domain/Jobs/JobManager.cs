using System;
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

        public async Task<Job> CreateAsync(string title, Guid organisationId)
        {
            return new Job(title, organisationId);
        }

        public async Task<Job> UpdateAsync(Guid userId, Job job, string title, string description)
        {
            job.Title = title;
            job.Description = description;

            return job;
        }

        // Add any other domain service methods
        // Note:    Do not create domain service methods simply to change the
        //          entity properties without any business logic.
    }
}
