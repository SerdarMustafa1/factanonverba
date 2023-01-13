using Collabed.JobPortal.Clients;
using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;

namespace JobPortal.Jobs
{
    public class JobAppService : ApplicationService, IJobAppService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IRepository<Client, Guid> _clientRepository;
        private readonly JobManager _jobManager;

        public JobAppService(IJobRepository jobRepository, JobManager jobManager, IRepository<Client, Guid> clientRepository)
        {
            _jobRepository = jobRepository;
            _jobManager = jobManager;
            _clientRepository = clientRepository;
        }

        public async Task<JobDto> GetAsync(Guid id)
        {
            var job = await _jobRepository.GetAsync(id);

            return ObjectMapper.Map<Job, JobDto>(job);
        }

        public async Task<PagedResultDto<JobDto>> GetListAsync(JobGetListInput input)
        {
            var jobs = await _jobRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount);
            var totalCount = await _jobRepository.CountAsync();

            return new PagedResultDto<JobDto>(totalCount, ObjectMapper.Map<List<Job>, List<JobDto>>(jobs));
        }

        [Authorize]
        public async Task<JobDto> CreateAsync(CreateJobDto input)
        {
            var job = await _jobManager.CreateAsync(CurrentUser.Id.Value, input.Title);
            job.Description = input.Description;

            var newJob = await _jobRepository.InsertAsync(job);
            return ObjectMapper.Map<Job, JobDto>(newJob);
        }

        [Authorize]
        public async Task UpdateAsync(Guid id, CreateUpdateJobDto input)
        {
            var job = await _jobRepository.GetAsync(id);

            var updatedJob = await _jobManager.UpdateAsync(CurrentUser.Id.Value,
                job,
                input.Title,
                input.Description
            );

            await _jobRepository.UpdateAsync(updatedJob);
        }

        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            await _jobRepository.DeleteAsync(id);
        }
    }
}
