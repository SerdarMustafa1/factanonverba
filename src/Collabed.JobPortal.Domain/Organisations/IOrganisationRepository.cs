using System;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Organisations
{
    public interface IOrganisationRepository
    {
        Task<Guid?> GetOrganisationIdByUserId(Guid userId);
    }
}
