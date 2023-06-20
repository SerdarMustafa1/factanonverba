using Collabed.JobPortal.Applications;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
	public interface IJobAppService
	{
		Task<JobDto> GetAsync(Guid id);
		Task<PagedResultDto<JobDto>> GetListAsync(JobGetListInput input);
		Task<JobDto> CreateAsync(CreateJobDto input);
		Task UpdateAsync(Guid id, CreateUpdateJobDto input);
		Task DeleteAsync(Guid id);
		Task<JobResponseDto> HandleExternalJobFeedAsync(ExternalJobRequest externalJobRequest);
		Task<JobDto> GetByReferenceAsync(string reference);
		Task<IEnumerable<CategorisedJobsDto>> GetCategorisedJobs();
		Task<PagedResultDto<JobDto>> SearchAsync(SearchCriteriaInput criteria, CancellationToken cancellationToken);
		Task<int?> GetApplicationStepsByJobReferenceAsync(string reference);
		Task FeedAllAdzunaJobsAsync();
		Task<IEnumerable<ScreeningQuestionDto>> ScreeningQuestionsByJobRefAsync(string jobReference);
		Task<IEnumerable<SupportingDocumentDto>> GetSupportingDocumentsByJobRefAsync(string jobReference);
		Task ApplyForAJob(ApplicationDto application);
		Task DeactivateJobAsync(string reference);
		Task<PagedResultDto<JobSummaryDto>> GetAllListAsync(JobGetListInput input);
		Task ReviewJobsAsync();
		Task<StatusedJobsDto> GetStatusedJobsCount(JobGetListInput input);
		Task<JobDto> GetByReferenceWithApplicationsAsync(string reference);
		Task ProgressJobApplicationAsync(string applicationId, bool nextStage);
		Task<bool?> HireApplicantAsync(string applicationId, string jobReference);
		Task NotifyApplicantsAsync(string reference);
		Task<object> ToggleJobStatusAsync(bool acceptingApplications, string jobReference);
		Task<ApplicationSummaryDto> GetApplicationByReferenceAsync(string reference);
		Task UpdateApplicantRatingAsync(string rating, string appReference);

		Task SetInterviewDateAsync(string interviewDate, string appReference);
	}
}
