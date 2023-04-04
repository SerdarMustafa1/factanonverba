using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Collabed.JobPortal.Organisations
{
    public class OrganisationAppService : ApplicationService, IOrganisationAppService
    {
        public readonly IOrganisationRepository _organisationRepository;

        public OrganisationAppService(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<OrganisationDto> CreateAsync(CreateOrganisationDto createOrganisationDto)
        {
            // TODO: Post-MVP - Update organisation creation flow
            var organisation = new Organisation(GuidGenerator.Create(), createOrganisationDto.Name, createOrganisationDto.Email);
            await _organisationRepository.InsertAsync(organisation);

            // For now user creating an organisation is automatically its member
            organisation.AddMember(createOrganisationDto.OwnerId);

            // TODO: Assign admin role to the user that created an organisation

            return ObjectMapper.Map<Organisation, OrganisationDto>(organisation);
        }
    }
}
