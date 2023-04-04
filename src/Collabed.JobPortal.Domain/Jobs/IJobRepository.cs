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
		Task<Job> GetByReferenceAsync(string reference);
		Task DeleteByReferenceAsync(string reference);
		Task<bool> CheckIfJobExistsByReference(string reference);
		Task<JobWithDetails> GetWithDetailsByReferenceAsync(string reference);
	}
}
