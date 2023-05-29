using Collabed.JobPortal.Account;
using System;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Organisations
{
    public interface IOrganisationAppService
    {
        Task<OrganisationDto> CreateAsync(CreateOrganisationDto createOrganisationDto);
        Task DeleteAllOrganisationDataAsync(Guid organisationId);
        Task<OrganisationDto> GetOrganisationByEmailAsync(string email);
        Task UpdateOrganisationProfile(UpdateCompanyProfileDto companyProfile);
    }
}
