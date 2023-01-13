using Collabed.JobPortal.Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace Collabed.JobPortal.Jobs
{
    public class JobManager : DomainService
    {
        private readonly IRepository<Job, Guid> _jobRepository;
        private readonly IRepository<Client, Guid> _clientRepository;


        public JobManager(IRepository<Job, Guid> jobRepository, IRepository<Client, Guid> clientRepository)
        {
            _jobRepository = jobRepository;
            _clientRepository = clientRepository;
        }

        public async Task<Job> CreateAsync(Guid userId, string title)
        {
            var client = await GetClientAsync(userId);

            return new Job(title, client);
        }

        public async Task<Job> UpdateAsync(Guid userId, Job job, string title, string description)
        {
            var client = await GetClientAsync(userId);

            if (client.Id != userId)
            {
                Logger.LogError($"User with id {userId} is not allowed to update the job with id {job.Id}");
                throw new UserFriendlyException("Only owner of the job is allowed to update its content.", logLevel: LogLevel.Error, code: "403");
            }

            job.Title = title;
            job.Description = description;

            return job;
        }

        // Add any other domain service methods
        // Note:    Do not create domain service methods simply to change the
        //          entity properties without any business logic.

        private async Task<Client> GetClientAsync(Guid userId)
        {
            var client = await _clientRepository.FindAsync(x => x.UserId == userId);
            if (client == null)
            {
                Logger.LogError($"Could not find a user with id {userId} in clients table.");
                throw new UserFriendlyException("Could not create a new job. Client has not been found.", details: $"UserId: {userId}", logLevel: LogLevel.Error);
            }

            return client;
        }
    }
}
