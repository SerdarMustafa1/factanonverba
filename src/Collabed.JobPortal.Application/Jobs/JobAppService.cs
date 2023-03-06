using Collabed.JobPortal;
using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Job;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace JobPortal.Jobs
{
    public class JobAppService : ApplicationService, IJobAppService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly JobManager _jobManager;
        private readonly ILogger<JobAppService> _logger;
        private readonly IdibuOptions _idibuOptions;
        private readonly BroadbeanOptions _broadBeanOptions;

        public JobAppService(IJobRepository jobRepository, JobManager jobManager, ILogger<JobAppService> logger, IOptions<IdibuOptions> idibuOptions, IOptions<BroadbeanOptions> broadBeanOptions, IRepository<Category> categoryRepository)
        {
            _jobRepository = jobRepository;
            _jobManager = jobManager;
            _logger = logger;
            _idibuOptions = idibuOptions.Value;
            _broadBeanOptions = broadBeanOptions.Value;
            _categoryRepository = categoryRepository;
        }

        public async Task<JobDto> GetAsync(Guid id)
        {
            var job = await _jobRepository.GetAsync(id);

            return ObjectMapper.Map<Job, JobDto>(job);
        }

        public async Task<JobDto> GetByReferenceAsync(string reference)
        {
            var job = await _jobRepository.GetByReferenceAsync(reference);

            return ObjectMapper.Map<Job, JobDto>(job);
        }

        public async Task<PagedResultDto<JobDto>> GetListAsync(JobGetListInput input)
        {
            var jobs = await _jobRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount);
            var totalCount = await _jobRepository.CountAsync();

            return new PagedResultDto<JobDto>(totalCount, ObjectMapper.Map<List<Job>, List<JobDto>>(jobs));
        }

        [Authorize(BmtPermissions.ManageJobs)]
        public async Task<JobDto> CreateAsync(CreateJobDto input)
        {
            var organisationClaim = CurrentUser.FindClaim(ClaimNames.OrganisationClaim);
            if (organisationClaim == null || string.IsNullOrEmpty(organisationClaim.Value))
            {
                throw new BusinessException("User is not allowed to post jobs. User is not a member of any organisation.");
            }
            var organisationId = Guid.Parse(organisationClaim.Value);

            var job = await _jobManager.CreateAsync(input.Title, organisationId);
            job.Description = input.Description;

            var newJob = await _jobRepository.InsertAsync(job);
            return ObjectMapper.Map<Job, JobDto>(newJob);
        }

        [Authorize(BmtPermissions.ManageJobs)]
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

        [Authorize(BmtPermissions.ManageJobs)]
        public async Task DeleteAsync(Guid id)
        {
            await _jobRepository.DeleteAsync(id);
        }

        public async Task<List<CategoryDto>> GetJobCategoriesAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);
        }

        [AllowAnonymous]
        [RemoteService(IsEnabled = false)]
        public async Task<JobResponseDto> HandleExternalJobFeedAsync(ExternalJobRequest externalJobRequest)
        {
            try
            {
                ValidateExternalRequestDataFormat(externalJobRequest);
                var jobOrigin = GetJobOrigin(externalJobRequest.Username, externalJobRequest.Password);
                Enum.TryParse(externalJobRequest.Command, out CommandType command);
                var message = "";
                var jobReference = externalJobRequest.Reference;
                switch (command)
                {
                    case CommandType.add:
                        var newJob = _jobManager.CreateExternal(jobReference);
                        newJob = ObjectMapper.Map<ExternalJobRequest, Job>(externalJobRequest);
                        await _jobRepository.InsertAsync(newJob);
                        message = $"Posted a new job with reference {jobReference}";
                        break;
                    case CommandType.update:
                        var existingJob = await _jobRepository.GetByReferenceAsync(jobReference);
                        ObjectMapper.Map<ExternalJobRequest, Job>(externalJobRequest, existingJob);
                        await _jobRepository.UpdateAsync(existingJob);
                        message = $"Updated a job with reference {jobReference}";
                        break;
                    case CommandType.delete:
                        await _jobRepository.DeleteByReferenceAsync(jobReference);
                        message = $"Deleted a job with reference {jobReference}";
                        break;
                }

                Logger.LogInformation(message);

                return new JobResponseDto(JobResponseCodes.Success, jobReference, message);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"External Job Feed validation failed with the message: {ae.Message}", ae.InnerException);
                return new JobResponseDto(JobResponseCodes.Failed, externalJobRequest.Reference, ae.Message);
            }
            catch (AuthenticationException ae)
            {
                _logger.LogError($"External Job Feed validation failed with the message: {ae.Message}", ae.InnerException);
                return new JobResponseDto(JobResponseCodes.Failed, externalJobRequest.Reference, ae.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error has occured when feeding an external job: {ex.Message}", ex.InnerException);
                return new JobResponseDto(JobResponseCodes.Failed, externalJobRequest.Reference, "Unexpected error has occured");
            }
        }

        private JobOrigin GetJobOrigin(string username, string password)
        {
            if (_idibuOptions.Username == username && _idibuOptions.Password == password)
            {
                return JobOrigin.Idibu;
            }

            if (_broadBeanOptions.Username == username && _broadBeanOptions.Password == password)
            {
                return JobOrigin.Broadbean;
            }

            throw new AuthenticationException("Username or password is incorrect.");
        }

        private void ValidateExternalRequestDataFormat(ExternalJobRequest jobRequest)
        {
            var validationErrors = new List<string>();
            if (!Enum.TryParse(typeof(CommandType), jobRequest.Command, true, out _))
                validationErrors.Add("Invalid Command value");
            if (!Enum.TryParse(typeof(JobType), jobRequest.Type, true, out _))
                validationErrors.Add("Invalid JobType value");
            if (!Enum.TryParse(typeof(SalaryPeriodType), jobRequest.SalaryPeriod, true, out _))
                validationErrors.Add("Invalid SalaryPeriodType value");

            if (validationErrors.Any())
                throw new ArgumentException(string.Join(",", validationErrors));
        }
    }
}
