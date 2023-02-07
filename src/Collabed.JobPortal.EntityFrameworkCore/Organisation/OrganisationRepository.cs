using Collabed.JobPortal.EntityFrameworkCore;
using Collabed.JobPortal.Organisations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Collabed.JobPortal.Organisation
{
    public class OrganisationRepository : EfCoreRepository<JobPortalDbContext, Organisations.Organisation, Guid>, IOrganisationRepository
    {
        public OrganisationRepository(IDbContextProvider<JobPortalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Guid?> GetOrganisationIdByUserId(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            var dbSet = (await GetDbContextAsync()).Set<OrganisationMember>();

            return dbSet.FirstOrDefault(x => x.UserId == userId)?.OrganisationId;
        }

        private async Task<IQueryable<Organisations.Organisation>> ApplyFilterAsync()
        {
            var dbContext = await GetDbContextAsync();

            return await GetDbSetAsync();
        }
    }
}
