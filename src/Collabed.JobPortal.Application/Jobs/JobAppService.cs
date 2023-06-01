using Collabed.JobPortal;
using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.Applications;
using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Extensions;
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
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace JobPortal.Jobs
{
    [ExposeServices(typeof(JobAppService), typeof(IJobAppService))]
    public class JobAppService : ApplicationService, IJobAppService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<JobApplicant> _jobApplicantsRepository;
        private readonly JobManager _jobManager;
        private readonly UserManager _userManager;
        private readonly ILogger<JobAppService> _logger;
        private readonly IdibuOptions _idibuOptions;
        private readonly BroadbeanOptions _broadBeanOptions;
        private readonly ISearchService _searchService;
        private readonly IFileAppService _fileAppService;
        private readonly IBmtAccountEmailer _bmtAccountEmailer;
        private readonly IRepository<UserProfile> _profileRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IRepository<SupportingDocument> _supportingDocumentRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<ScreeningQuestion> _screeningQuestionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public JobAppService(IJobRepository jobRepository, JobManager jobManager, ILogger<JobAppService> logger,
            IOptions<IdibuOptions> idibuOptions, IOptions<BroadbeanOptions> broadBeanOptions, IOrganisationRepository organisationRepository,
            IRepository<Category> categoriesRepository, ISearchService searchService, IFileAppService fileAppService,
            IBmtAccountEmailer bmtAccountEmailer, IRepository<UserProfile> profileRepository, IRepository<IdentityUser, Guid> userRepository, UserManager userManager, IRepository<SupportingDocument> supportingDocumentRepository, IRepository<Location> locationRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<JobApplicant> jobApplicantsRepository, IRepository<ScreeningQuestion> screeningQuestionRepository)
        {
            _jobRepository = jobRepository;
            _jobManager = jobManager;
            _logger = logger;
            _idibuOptions = idibuOptions.Value;
            _broadBeanOptions = broadBeanOptions.Value;
            _organisationRepository = organisationRepository;
            _categoryRepository = categoriesRepository;
            _searchService = searchService;
            _locationRepository = locationRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _fileAppService = fileAppService;
            _bmtAccountEmailer = bmtAccountEmailer;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _supportingDocumentRepository = supportingDocumentRepository;
            _jobApplicantsRepository = jobApplicantsRepository;
            _screeningQuestionRepository = screeningQuestionRepository;
        }

        [RemoteService(IsEnabled = false)]
        public async Task FeedAllAdzunaJobsAsync()
        {
            await _jobRepository.DeleteAsync(x => x.JobOrigin == JobOrigin.Adzuna, true);

            var architectureJobs = new string[] { "Architect", "CAD Technician", "Interior Designer", "Exhibition Designer" };
            foreach (var job in architectureJobs)
            {
                await GetAdzunaJobsAsync(job, "creative-design-jobs", 2);
                await GetAdzunaJobsAsync(job, "trade-construction-jobs", 2);
                await GetAdzunaJobsAsync(job, "engineering-jobs", 2);
            }
            var constructionJobs = new string[] { "Acoustics Consultant", "Building Control Officer", "Building Site Inspector",
                "Conservation Officer", "Construction Contracts Manager", "Construction Plant Hire Adviser", "Construction Manager",
                "Construction Site Supervisor", "Project Manager", "Site Manager", "Estimator", "Facilities Manager"};
            foreach (var job in constructionJobs)
            {
                await GetAdzunaJobsAsync(job, "trade-construction-jobs", 3);
            }

            var assesorJobs = new string[] { "Energy Assessor", "Fire Risk Assessor" };
            foreach (var job in assesorJobs)
            {
                await GetAdzunaJobsAsync(job, "trade-construction-jobs", 1);
                await GetAdzunaJobsAsync(job, "engineering-jobs", 1);
            }
            var engineeringJobs = new string[] { "Building Services", "Building Engineer", "Civil Engineer", "Thermal Insulation Engineer",
                "Engineering Construction", "Heating Engineer", "Ventilation Engineer"};
            foreach (var job in engineeringJobs)
            {
                await GetAdzunaJobsAsync(job, "trade-construction-jobs", 4);
                await GetAdzunaJobsAsync(job, "engineering-jobs", 4);
            }

            var surveyingJobs = new string[] { "Surveyor", "Heritage Building", "Building Surveying", "Survey Technician", "Surveying" };
            foreach (var job in surveyingJobs)
            {
                await GetAdzunaJobsAsync(job, "trade-construction-jobs", 6);
            }
            var planningJobs = new string[] { "Construction Planning", "Planning Engineer", "Heritage Officer", "Heritage Assistant",
                "Planning and Development Surveyor", "Town Planner", "Urban Planner"};
            foreach (var job in planningJobs)
            {
                await GetAdzunaJobsAsync(job, "", 5);
            }
        }

        #region Adzuna private methods
        private async Task<int?> GetAdzunaJobsAsync(string title, string category, int catId)
        {
            var locations = await _locationRepository.GetListAsync();
            var page = 1;
            int count;
            int? response = await FeedAdzunaJobsAsync(page, title, category, catId, locations);
            count = page*50;
            page++;

            while (response != null && response.Value > count)
            {
                response = await FeedAdzunaJobsAsync(page, title, category, catId, locations);
                count = page*50;
                page++;
            }

            return response;
        }

        private async Task<int?> FeedAdzunaJobsAsync(int page, string title, string category, int catId, List<Location> locations)
        {
            title = Uri.EscapeDataString(title);
            var whatExclude = "software%20microsoft%20data%20migration%20systems%20solution%20integration%20security%20enterprise%20infrastructure%20service%20DBRE";
            var adzunaJobSearchUrl = new StringBuilder();
            adzunaJobSearchUrl.Append(@$"http://api.adzuna.com:80/v1/api/jobs/gb/search/{page}?");
            adzunaJobSearchUrl.Append("app_id=10dbdc09&app_key=5bc51c49c3c47b753ed62f8165a85d00");
            if (!string.IsNullOrEmpty(whatExclude))
                adzunaJobSearchUrl.Append($"&what_exclude={whatExclude}");

            adzunaJobSearchUrl.Append($"&title_only={title}");
            if (!string.IsNullOrEmpty(category))
                adzunaJobSearchUrl.Append($"&category={category}");

            adzunaJobSearchUrl.Append("&salary_include_unknown=1&results_per_page=500");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync(adzunaJobSearchUrl.ToString());
            var resultUtf8 = await response.Content.ReadAsStreamAsync();
            var jobsResponse = JsonSerializer.Deserialize<AdzunaJobResponse>(resultUtf8);

            using (var uow = _unitOfWorkManager.Begin(
                isTransactional: true))
            {
                if (jobsResponse?.Count > 0)
                {
                    foreach (var item in jobsResponse.Results)
                    {
                        if ((await _jobRepository.GetByReferenceAsync(item.Reference)) != null)
                            continue;

                        var newJob = _jobManager.CreateExternal(item.Reference);
                        ObjectMapper.Map<AdzunaJobResult, Job>(item, newJob);
                        _jobManager.ConvertSalaryRates(newJob);

                        if (DateTime.TryParseExact(item.PostDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out var postDate))
                        {
                            newJob.CreationTime = postDate;
                        }

                        newJob.JobOrigin = JobOrigin.Adzuna;
                        newJob.OfficeLocationId = MapOfficeLocation(item.JobLocation?.Name, locations);
                        newJob.CategoryId = catId;
                        await _jobRepository.InsertAsync(newJob);
                    }
                }
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }

            return jobsResponse?.Count;
        }

        private static int? MapOfficeLocation(string name, List<Location> locations)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var locationSplit = name.Split(',');
            var foundLocation = locations.FirstOrDefault(x => x.Name == locationSplit[0].TrimEnd().TrimStart());
            if (foundLocation == null)
            {
                if (locationSplit.Length <= 1)
                    return null;
            }
            else
            {
                return foundLocation.Id;
            }

            return locations.FirstOrDefault(x => x.Name == locationSplit[1].TrimEnd().TrimStart())?.Id;
        }

        #endregion

        public async Task<int?> GetApplicationStepsByJobReferenceAsync(string reference)
        {
            var steps = 2; // Contact details + right to work in UK

            var job = await _jobRepository.GetJobForApplyByReferenceAsync(reference);

            if (job.ScreeningQuestions.Any())
            {
                steps++;
            }

            if (job.JobOrigin != JobOrigin.Native)
            {
                // For external jobs we want to collect CV, Cover letter and portfolio
                steps += 3;
            }
            else
            {
                steps += job.SupportingDocuments.Count;
            }

            return steps;
        }

        public async Task<PagedResultDto<JobDto>> SearchAsync(SearchCriteriaInput criteria, CancellationToken cancellationToken)
        {
            var mapSearchResult = new MapSearchResult();
            if (!string.IsNullOrEmpty(criteria.Location))
            {
                mapSearchResult = await _searchService.GetLocationCoordinatesAsync(criteria.Location, cancellationToken);
            }

            var jobs = await _jobRepository.GetListBySearchCriteriaAsync(criteria.Sorting, criteria.SkipCount, criteria.MaxResultCount,
                criteria.Categories, criteria.Keyword, mapSearchResult.ResultsFound, (mapSearchResult?.Latitude, mapSearchResult?.Longitude), criteria.SearchRadius, criteria.NetZero, criteria.ContractType,
                criteria.EmploymentType, criteria.Workplace, criteria.SalaryMinimum, criteria.SalaryMaximum, cancellationToken);
            var totalCount = await _jobRepository.CountBySearchCriteriaAsync(criteria.Sorting, criteria.SkipCount, criteria.MaxResultCount,
                criteria.Categories, criteria.Keyword, mapSearchResult.ResultsFound, (mapSearchResult?.Latitude, mapSearchResult?.Longitude), criteria.SearchRadius, criteria.NetZero, criteria.ContractType,
                criteria.EmploymentType, criteria.Workplace, criteria.SalaryMinimum, criteria.SalaryMaximum, cancellationToken);

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
            var jobs = new List<Job>();
            int totalCount = 0;
            if (input.OrganisationId.HasValue)
            {
                jobs = await _jobRepository.GetListByOrganisationIdAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.OrganisationId.Value);
                totalCount = await _jobRepository.CountAsync(x => x.OrganisationId == input.OrganisationId && x.Status != JobStatus.Deleted);
            }
            else
            {
                jobs = await _jobRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount);
                totalCount = await _jobRepository.CountAsync();
            }

            return new PagedResultDto<JobDto>(totalCount, ObjectMapper.Map<List<Job>, List<JobDto>>(jobs));
        }

        public async Task<IEnumerable<CategorisedJobsDto>> GetCategorisedJobs()
        {
            var categories = await _categoryRepository.GetListAsync();
            var categorisedJobs = new List<CategorisedJobsDto>();
            foreach (var category in categories)
            {
                var jobs = await _jobRepository.CountAsync(x => x.CategoryId == category.Id && x.ApplicationDeadline > DateTime.UtcNow);
                categorisedJobs.Add(new CategorisedJobsDto { Id = category.Id, JobsCount = jobs, Name = category.Name });
            }

            return categorisedJobs;
        }

        [Authorize(BmtPermissions.ManageJobs)]
        public async Task<JobDto> CreateAsync(CreateJobDto input)
        {
            var organisationId = ExtractOrganisationId();
            var job = await _jobManager.CreateAsync(organisationId);

            IEnumerable<Collabed.JobPortal.DropDowns.ScreeningQuestion> screeningQuestions = null;
            if (input.ScreeningQuestions != null && input.ScreeningQuestions.Any())
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

        private Guid ExtractOrganisationId()
        {
            var organisationClaim = CurrentUser.FindClaim(ClaimNames.OrganisationClaim);
            if (organisationClaim == null || string.IsNullOrEmpty(organisationClaim.Value))
            {
                throw new BusinessException(message: "You are not allowed to perform this action. User is not a member of any organisation.");
            }

            var organisationId = Guid.Parse(organisationClaim.Value);
            return organisationId;
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

        [Authorize(BmtPermissions.ManageJobs)]
        public async Task DeactivateJobAsync(string reference)
        {
            var organisationId = ExtractOrganisationId();

            var job = await _jobRepository.GetByReferenceAsync(reference);
            if (job == null)
            {
                throw new BusinessException(message: "The job has already been deleted.");
            }
            if (job.OrganisationId != organisationId)
            {
                throw new BusinessException(message: "Job can only be deleted by the user that has created it.");
            }
            job.Status = JobStatus.Deleted;
            job.Applicants = null;

            await _jobApplicantsRepository.DeleteAsync(x => x.JobId == job.Id);
            await DeleteScreeningQuestionsAsync(job.Id);
            await _jobRepository.UpdateAsync(job);
        }

        public async Task<bool> CheckIfAlreadyAppliedAsync(string jobRef, Guid userId)
        {
            var job = await _jobRepository.GetByReferenceAsync(jobRef);
            if (job != null && job.Applicants.Any(x => x.UserId == userId))
            {
                return true;
            }

            return false;
        }

        [Authorize(BmtPermissions.ApplyForJobs)]
        public async Task ApplyForAJob(ApplicationDto application)
        {
            var blobFileName = string.Empty;
            var job = await _jobRepository.GetByReferenceAsync(application.JobReference);
            if (job == null)
            {
                throw new UserFriendlyException("The job you're applying for has not been found.");
            }
            if (job.Applicants.Any(x => x.UserId== application.UserId))
            {
                throw new UserFriendlyException("You have already applied for this job.");
            }

            var userProfile = await _profileRepository.FindAsync(x => x.UserId == application.UserId);
            if (userProfile == null)
            {
                userProfile = _userManager.CreateUserProfile(application.UserId);
                userProfile = await _profileRepository.InsertAsync(userProfile);
            }
            var cvContentType = application.CvContentType;
            var cvFileName = application.CvFileName;

            if (application.IsNewCvAttached)
            {
                if (!string.IsNullOrEmpty(cvFileName) &&
                    !string.IsNullOrEmpty(cvContentType) && application.CvContent != null)
                {
                    blobFileName = RandomNameGenerator.GenerateRandomName(10);
                    await _fileAppService.SaveBlobAsync(new SaveBlobInputDto { Name = blobFileName, Content = application.CvContent });
                }
            }
            else
            {
                blobFileName = userProfile.CvBlobName;
                cvContentType = userProfile.CvContentType;
                cvFileName = userProfile.CvFileName;
            }

            await UpdateUserProfile(userProfile, application, blobFileName);
            var screeningAnswers = application.ScreeningQuestions.Any() ? application.ScreeningQuestions.Select(x => (x.Id, x.Answer)) : default;
            job.AddApplicant(application.UserId, blobFileName, cvContentType, cvFileName, application.PortfolioLink, application.CoverLetter, screeningAnswers);

            await _jobRepository.UpdateAsync(job);

            await ProcessApplicationEmailsAsync(application, job, blobFileName);
        }

        private async Task ProcessApplicationEmailsAsync(ApplicationDto application, Job job, string blobFileName)
        {
            var emailApplication = ObjectMapper.Map<ApplicationDto, ApplicationEmailDto>(application);
            emailApplication.JobPosition = job.Title;
            emailApplication.CvBlobName = blobFileName;

            if (job.OrganisationId.HasValue && job.JobOrigin == JobOrigin.Native)
            {
                var organisation = await _organisationRepository.FindAsync(job.OrganisationId.Value);
                if (organisation == null)
                    throw new BusinessException(message: "Failed to find the organisation that posted a job");

                emailApplication.CompanyName = organisation.Name;
                emailApplication.CompanyEmail = organisation.EmailAddress;
                await _bmtAccountEmailer.SendApplicationEmailToCompanyAsync(emailApplication, true);
            }

            if ((job.JobOrigin == JobOrigin.Idibu && string.IsNullOrEmpty(job.ApplicationUrl)) || job.JobOrigin == JobOrigin.Broadbean)
            {
                emailApplication.CompanyName = job.CompanyName;
                emailApplication.CompanyEmail = job.ApplicationEmail;
                await _bmtAccountEmailer.SendApplicationEmailToCompanyAsync(emailApplication, false);
            }

            await _bmtAccountEmailer.SendApplicationConfirmationAsync(new ApplicationConfirmationDto
            {
                FirstName= application.FirstName,
                LastName= application.LastName,
                Email = application.Email,
                CompanyName = emailApplication.CompanyName,
                JobReference = job.Reference,
                JobTitle = job.Title
            });
        }

        public async Task<IEnumerable<SupportingDocumentDto>> GetSupportingDocumentsByJobRefAsync(string jobReference)
        {
            var supportingDocIds = await _jobRepository.GetSupportingDocumentsByReferenceAsync(jobReference);
            var supportingDocs = await _supportingDocumentRepository.GetListAsync(x => supportingDocIds.Contains(x.Id));
            return ObjectMapper.Map<IEnumerable<SupportingDocument>, IEnumerable<SupportingDocumentDto>>(supportingDocs);
        }

        public async Task<IEnumerable<ScreeningQuestionDto>> ScreeningQuestionsByJobRefAsync(string jobReference)
        {
            var screeningQuestions = await _jobRepository.GetScreeningQuestionsByReferenceAsync(jobReference);
            return ObjectMapper.Map<IEnumerable<ScreeningQuestion>, IEnumerable<ScreeningQuestionDto>>(screeningQuestions);
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
                        await DeleteJobAsync(jobReference);
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

        private async Task DeleteJobAsync(string jobReference)
        {
            var job = await _jobRepository.GetByReferenceAsync(jobReference);
            if (job == null)
            {
                throw new BusinessException(message: "The job has already been deleted.");
            }
            await _jobApplicantsRepository.DeleteAsync(x => x.JobId == job.Id);
            await _jobRepository.DeleteByReferenceAsync(jobReference);
        }

        private async Task UpdateUserProfile(UserProfile userProfile, ApplicationDto application, string blobFileName)
        {
            userProfile.PostCode= application.PostCode;
            if (application.IsNewCvAttached)
            {
                userProfile.CvFileName = application.CvFileName;
                userProfile.CvContentType = application.CvContentType;
                userProfile.CvBlobName = blobFileName;
            }
            var user = await _userRepository.GetAsync(x => x.Id == application.UserId);
            user.Name = application.FirstName;
            user.Surname = application.LastName;
            user.SetPhoneNumber(application.PhoneNumber, false);

            await _userRepository.UpdateAsync(user);
        }

        private async Task<string> UpdateJobAsync(ExternalJobRequest externalJobRequest, string message, string jobReference)
        {
            var existingJob = await _jobRepository.GetByReferenceAsync(jobReference);
            if (existingJob == null)
            {
                throw new BusinessException(message: $"The job with reference '{jobReference}' could not be found.");
            }
            ObjectMapper.Map<ExternalJobRequest, Job>(externalJobRequest, existingJob);
            _jobManager.ConvertSalaryRates(existingJob);
            var locations = await _locationRepository.GetListAsync();
            existingJob.OfficeLocationId = MapOfficeLocation(externalJobRequest.Location, locations);
            await DeleteScreeningQuestionsAsync(existingJob.Id);
            existingJob.ScreeningQuestions = _jobManager.CreateScreeningQuestions(ConvertScreeningQuestions(externalJobRequest.ScreeningQuestions), existingJob.Id);
            await _jobRepository.UpdateAsync(existingJob);
            message = $"Updated a job with reference {jobReference}";

            return message;
        }

        private async Task DeleteScreeningQuestionsAsync(Guid jobId)
        {
            var screeningQuestions = await _screeningQuestionRepository.GetListAsync(x => x.JobId == jobId);
            if (screeningQuestions.Any())
                await _screeningQuestionRepository.DeleteManyAsync(screeningQuestions);
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
            var locations = await _locationRepository.GetListAsync();
            newJob.OfficeLocationId = MapOfficeLocation(externalJobRequest.Location, locations);
            _jobManager.ConvertSalaryRates(newJob);

            if (externalJobRequest.ScreeningQuestions != null)
            {
                newJob.ScreeningQuestions = _jobManager.CreateScreeningQuestions(ConvertScreeningQuestions(externalJobRequest.ScreeningQuestions), newJob.Id);
            }
            await _jobRepository.InsertAsync(newJob);

            message = $"Posted a new job with reference {jobReference}";

            return message;
        }

        private static IEnumerable<(string, bool?)> ConvertScreeningQuestions(IEnumerable<Collabed.JobPortal.Jobs.ExtScreeningQuestion> screeningQuestions)
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
                        // Convert CorrectAnswer to AutoReject
                        "yes" => false,
                        "no" => true,
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
                throw new BusinessException(message: "Job with the same reference has already been added.");
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
                throw new BusinessException(message: $"Insufficient credits to post a job to BuildMyTalent job board");
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
