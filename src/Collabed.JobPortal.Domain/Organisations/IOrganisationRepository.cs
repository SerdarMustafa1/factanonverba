using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Collabed.JobPortal.Organisations
{
    public interface IOrganisationRepository : IRepository<Organisation, Guid>
    {
        Task<Guid?> GetOrganisationIdByUserIdAsync(Guid userId);
        Task<Guid?> GetOrganisationByEmailAsync(string contactEmail);
        Task<bool> DeductCreditsForJobPosting(Guid organisationId, int credits);
    }
}
