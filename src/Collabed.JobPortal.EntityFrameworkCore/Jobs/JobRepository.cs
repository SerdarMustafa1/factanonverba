using Collabed.JobPortal.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Collabed.JobPortal.Jobs
{
    public class JobRepository : EfCoreRepository<JobPortalDbContext, Job, Guid>, IJobRepository
    {
        public JobRepository(IDbContextProvider<JobPortalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Job>> GetListAsync(string sorting, int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilterAsync();

            return await query
                .OrderBy(!string.IsNullOrWhiteSpace(sorting) ? sorting : nameof(Job.Title))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        private async Task<IQueryable<Job>> ApplyFilterAsync()
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync());
        }
    }
}
