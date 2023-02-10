using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Collabed.JobPortal.Organisations
{
    public interface IOrganisationRepository : IRepository<Organisation, Guid>
    {
        Task<Guid?> GetOrganisationIdByUserId(Guid userId);
    }
}
