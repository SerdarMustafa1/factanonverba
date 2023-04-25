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

	}
}
