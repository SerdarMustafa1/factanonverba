using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Collabed.JobPortal.Jobs
{
	public interface IJobRepository : IRepository<Job, Guid>
	{
		Task<List<Job>> GetListAsync(string sorting, int skipCount, int maxResultCount, CancellationToken cancellationToken = default);
		Task<List<Job>> GetListByOrganisationIdAsync(string sorting, int skipCount, int maxResultCount, Guid organisationId, CancellationToken cancellationToken = default);

		Task<Job> GetByReferenceAsync(string reference);
		Task DeleteByReferenceAsync(string reference);
		Task<bool> CheckIfJobExistsByReference(string reference);
		Task<JobWithDetails> GetWithDetailsByReferenceAsync(string reference);
		Task<List<JobWithDetails>> GetListBySearchCriteriaAsync(string sorting, int skipCount, int maxResultCount, IEnumerable<int> categories,
			string keyword, bool locationsFound, (decimal? lat, decimal? lon) location, int? searchRadius, int? netZero, ContractType? contractType, EmploymentType? employmentType,
			JobLocation? workplace, int? salaryMinimum, int? salaryMaximum, CancellationToken cancellationToken = default);
		Task<int> CountBySearchCriteriaAsync(string sorting, int skipCount, int maxResultCount, IEnumerable<int> categories,
			string keyword, bool locationsFound, (decimal? lat, decimal? lon) location, int? searchRadius, int? netZero, ContractType? contractType, EmploymentType? employmentType,
			JobLocation? workplace, int? salaryMinimum, int? salaryMaximum, CancellationToken cancellationToken = default);
		Task<IEnumerable<int>> GetSupportingDocumentsByReferenceAsync(string reference);
		Task<IEnumerable<ScreeningQuestion>> GetScreeningQuestionsByReferenceAsync(string reference);
		Task<Job> GetJobForApplyByReferenceAsync(string reference);
		Task<List<Job>> GetAllJobsByOrganisationIdAsync(string search, JobStatus? status, string sorting, int skipCount, int maxResultCount, Guid organisationId, CancellationToken cancellationToken = default);
		Task UpdateExpiredJobsStatus();
	}
}
