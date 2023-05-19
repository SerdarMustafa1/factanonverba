using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Organisations
{
    public class OrganisationAppService : ApplicationService, IOrganisationAppService
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IRepository<JobApplicant> _jobApplicantsRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IRepository<ScreeningQuestion> _screeningQuestionRepository;
        private readonly IdentityUserManager _userManager;

        public OrganisationAppService(IOrganisationRepository organisationRepository, IRepository<JobApplicant> jobApplicantsRepository, IJobRepository jobRepository, IRepository<ScreeningQuestion> screeningQuestionRepository, IdentityUserManager userManager)
        {
            _organisationRepository = organisationRepository;
            _jobApplicantsRepository = jobApplicantsRepository;
            _jobRepository = jobRepository;
            _screeningQuestionRepository = screeningQuestionRepository;
            _userManager = userManager;
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

        [Authorize(BmtPermissions.Admin)]
        public async Task DeleteAllOrganisationDataAsync(Guid organisationId)
        {
            try
            {
                var jobs = await _jobRepository.GetListByOrganisationIdAsync("", 0, 10000, organisationId);
                foreach (var job in jobs)
                {
                    // Delete jobs and applicants
                    await _jobApplicantsRepository.DeleteAsync(x => x.JobId == job.Id);
                    await _screeningQuestionRepository.DeleteAsync(x => x.JobId == job.Id);
                    await _jobRepository.DeleteAsync(job);
                }

                // Delete organisation and user
                var organisation = await _organisationRepository.GetOrganisationWithMembersAsync(organisationId);
                var userId = organisation.Members.Select(x => x.UserId).FirstOrDefault();
                await _organisationRepository.DeleteAsync(organisationId);
                var user = await _userManager.GetByIdAsync(userId);
                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
