using Collabed.JobPortal;
using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Search;
using Collabed.JobPortal.Types;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
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
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly JobManager _jobManager;
        private readonly ILogger<JobAppService> _logger;
        private readonly IdibuOptions _idibuOptions;
        private readonly BroadbeanOptions _broadBeanOptions;
        private readonly ISearchService _searchService;

        public JobAppService(IJobRepository jobRepository, JobManager jobManager, ILogger<JobAppService> logger, IOptions<IdibuOptions> idibuOptions, IOptions<BroadbeanOptions> broadBeanOptions, IOrganisationRepository organisationRepository, IRepository<Category> categoriesRepository, ISearchService searchService)
        {
            _jobRepository = jobRepository;
            _jobManager = jobManager;
            _logger = logger;
            _idibuOptions = idibuOptions.Value;
            _broadBeanOptions = broadBeanOptions.Value;
            _organisationRepository = organisationRepository;
            _categoryRepository = categoriesRepository;
            _searchService = searchService;
        }

        public async Task<PagedResultDto<JobDto>> SearchAsync(SearchCriteriaInput criteria, CancellationToken cancellationToken)
        {
            MapSearchResult mapSearchResult = null;
            if (!string.IsNullOrEmpty(criteria.Location))
            {
                mapSearchResult = await _searchService.GetLocationCoordinatesAsync(criteria.Location, cancellationToken);
            }

            var jobs = await _jobRepository.GetListBySearchCriteriaAsync(criteria.Sorting, criteria.SkipCount, criteria.MaxResultCount,
                criteria.CategoryId, criteria.Keyword, mapSearchResult.ResultsFound, (mapSearchResult?.Latitude, mapSearchResult?.Longitude), criteria.SearchRadius, criteria.NetZero, criteria.ContractType,
                criteria.EmploymentType, criteria.Workplace, criteria.SalaryMinimum, criteria.SalaryMaximum, cancellationToken);
            var totalCount = await _jobRepository.CountAsync();

            return new PagedResultDto<JobDto>(totalCount, ObjectMapper.Map<List<JobWithDetails>, List<JobDto>>(jobs));
        }

        public async Task<JobDto> GetAsync(Guid id)
        {
            var job = await _jobRepository.GetAsync(id);

            return ObjectMapper.Map<Job, JobDto>(job);
        }

        public async Task<JobDto> GetByReferenceAsync(string reference)
        {
            var job = await _jobRepository.GetWithDetailsByReferenceAsync(reference);

            return ObjectMapper.Map<JobWithDetails, JobDto>(job);
        }

        public async Task<PagedResultDto<JobDto>> GetListAsync(JobGetListInput input)
        {
            var jobs = await _jobRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount);
            var totalCount = await _jobRepository.CountAsync();

            return new PagedResultDto<JobDto>(totalCount, ObjectMapper.Map<List<Job>, List<JobDto>>(jobs));
        }

        public async Task<IEnumerable<CategorisedJobsDto>> GetCategorisedJobs()
        {
            var categories = await _categoryRepository.GetListAsync();
            var categorisedJobs = new List<CategorisedJobsDto>();
            foreach (var category in categories)
            {
                var jobs = await _jobRepository.CountAsync(x => x.CategoryId == category.Id);
                categorisedJobs.Add(new CategorisedJobsDto { Id = category.Id, JobsCount = jobs, Name = category.Name });
            }

            return categorisedJobs;
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
            var job = await _jobManager.CreateAsync(organisationId);

            IEnumerable<Collabed.JobPortal.DropDowns.ScreeningQuestion> screeningQuestions = null;
            if (input.ScreeningQuestions != null)
            {
                screeningQuestions = _jobManager.CreateScreeningQuestions(input.ScreeningQuestions, job.Id);
            }

            ObjectMapper.Map(input, job);
            job.SetSupportingDocs(input.SupportingDocuments)
               .ScreeningQuestions = screeningQuestions ?? null;
            _jobManager.ConvertSalaryRates(job);

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
                        message = await InsertNewJobAsync(externalJobRequest, jobOrigin, message, jobReference);
                        break;
                    case CommandType.update:
                        message = await UpdateJobAsync(externalJobRequest, message, jobReference);
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
            catch (BusinessException be)
            {
                _logger.LogError($"External Job Feed failed with the message: {be.Message}", be.InnerException);
                return new JobResponseDto(JobResponseCodes.Failed, externalJobRequest.Reference, be.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error has occured when feeding an external job: {ex.Message}", ex.InnerException);
                return new JobResponseDto(JobResponseCodes.Failed, externalJobRequest.Reference, "Unexpected error has occured");
            }
        }

        private async Task<string> UpdateJobAsync(ExternalJobRequest externalJobRequest, string message, string jobReference)
        {
            var existingJob = await _jobRepository.GetByReferenceAsync(jobReference);
            ObjectMapper.Map<ExternalJobRequest, Job>(externalJobRequest, existingJob);
            _jobManager.ConvertSalaryRates(existingJob);

            existingJob.ScreeningQuestions = _jobManager.CreateScreeningQuestions(ConvertScreeningQuestions(externalJobRequest.ScreeningQuestions), existingJob.Id);
            await _jobRepository.UpdateAsync(existingJob);
            message = $"Updated a job with reference {jobReference}";

            return message;
        }

        private async Task<string> InsertNewJobAsync(ExternalJobRequest externalJobRequest, JobOrigin jobOrigin, string message, string jobReference)
        {
            await CheckIfJobReferenceExistsAsync(externalJobRequest.Reference);
            // TODO: Implement post MVP, once business requirements are specified
            //var organisationId = await GetOrganisationAsync(externalJobRequest.ContactEmail);
            //await DeductOrganisationsCredits(organisationId, externalJobRequest.ContactEmail);

            var newJob = _jobManager.CreateExternal(jobReference);
            ObjectMapper.Map<ExternalJobRequest, Job>(externalJobRequest, newJob);
            newJob.JobOrigin = jobOrigin;
            _jobManager.ConvertSalaryRates(newJob);

            if (externalJobRequest.ScreeningQuestions != null)
            {
                newJob.ScreeningQuestions = _jobManager.CreateScreeningQuestions(ConvertScreeningQuestions(externalJobRequest.ScreeningQuestions), newJob.Id);
            }
            await _jobRepository.InsertAsync(newJob);

            message = $"Posted a new job with reference {jobReference}";

            return message;
        }

        private static IEnumerable<(string, bool?)> ConvertScreeningQuestions(IEnumerable<Collabed.JobPortal.Jobs.ScreeningQuestion> screeningQuestions)
        {
            foreach (var item in screeningQuestions)
            {
                bool? answer;
                if (string.IsNullOrEmpty(item.CorrectAnswer))
                {
                    answer = null;
                }
                else
                {
                    answer =item.CorrectAnswer.ToLower() switch
                    {
                        "yes" => true,
                        "no" => false,
                        _ => null,
                    };
                }
                yield return (item.Question, answer);
            }
        }

        private async Task CheckIfJobReferenceExistsAsync(string reference)
        {
            if (await _jobRepository.CheckIfJobExistsByReference(reference))
            {
                throw new BusinessException("Job with the same reference has already been added.");
            }
        }

        private async Task<Guid> GetOrganisationAsync(string contactEmail)
        {
            var organisationId = await _organisationRepository.GetOrganisationByEmailAsync(contactEmail);
            if (organisationId == null)
            {
                throw new ArgumentException($"Organisation doesn't exist in BuildMyTalent job board.", contactEmail);
            }

            return organisationId.Value;
        }

        private async Task DeductOrganisationsCredits(Guid organisationId, string contactEmail)
        {
            // TODO: Specify credits amount 
            var creditsDeducted = await _organisationRepository.DeductCreditsForJobPosting(organisationId, 1);

            if (!creditsDeducted)
            {
                throw new BusinessException($"Insufficient credits to post a job to BuildMyTalent job board");
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
                validationErrors.Add("Invalid command value");
            if (!Enum.TryParse(typeof(ContractType), jobRequest.Type, true, out _))
                validationErrors.Add("Invalid job_type value");
            var salaryPeriods = new string[] { "hour", "day", "week", "month", "annum" };
            if (!salaryPeriods.Contains(jobRequest.SalaryPeriod))
                validationErrors.Add("Invalid salary_per value");

            if (validationErrors.Any())
                throw new ArgumentException(string.Join(",", validationErrors));
        }
    }
}
