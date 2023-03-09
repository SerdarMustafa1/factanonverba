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

        public async Task<Guid?> GetOrganisationIdByUserIdAsync(Guid userId)
        {
            var dbSet = (await GetDbContextAsync()).Set<OrganisationMember>();

            return dbSet.FirstOrDefault(x => x.UserId == userId)?.OrganisationId;
        }

        public async Task<Guid?> GetOrganisationByEmailAsync(string contactEmail)
        {
            var dbSet = await ApplyFilterAsync();
            return dbSet.FirstOrDefault(x => x.EmailAddress == contactEmail)?.Id;
        }

        public async Task<bool> DeductCreditsForJobPosting(Guid organisationId, int credits)
        {
            //TODO: Implement once logic is known
            return true;
        }

        private async Task<IQueryable<Organisations.Organisation>> ApplyFilterAsync()
        {
            return await GetDbSetAsync();
        }
    }
}
