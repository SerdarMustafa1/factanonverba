using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Collabed.JobPortal.Organisations
{
    public interface IOrganisationRepository : IRepository<Organisation, Guid>
    {
        Task<Guid?> GetOrganisationIdByUserIdAsync(Guid userId);
        Task<Guid?> GetOrganisationByEmailAsync(string contactEmail);
        Task<bool> DeductCreditsForJobPosting(Guid organisationId, int credits);
        Task<List<Organisations.Organisation>> GetPagedListWithDetailsAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default);
        Task<Organisations.Organisation> GetOrganisationWithMembersAsync(Guid organisationId);
    }
}
