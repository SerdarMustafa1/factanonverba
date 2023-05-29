using Collabed.JobPortal.Account;
using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
        private readonly IFileAppService _fileAppService;
        private readonly ILogger<OrganisationAppService> _logger;

        public OrganisationAppService(IOrganisationRepository organisationRepository, IRepository<JobApplicant> jobApplicantsRepository, IJobRepository jobRepository, IRepository<ScreeningQuestion> screeningQuestionRepository, IdentityUserManager userManager, IFileAppService fileAppService, ILogger<OrganisationAppService> logger)
        {
            _organisationRepository = organisationRepository;
            _jobApplicantsRepository = jobApplicantsRepository;
            _jobRepository = jobRepository;
            _screeningQuestionRepository = screeningQuestionRepository;
            _userManager = userManager;
            _fileAppService = fileAppService;
            _logger = logger;
        }

        public async Task<OrganisationDto> GetOrganisationByEmailAsync(string email)
        {
            var organisation = await _organisationRepository.FindAsync(x => x.EmailAddress == email);
            if (organisation == null)
            {
                return default;
            }

            return ObjectMapper.Map<Organisation, OrganisationDto>(organisation);
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
                _logger.LogException(ex);
                throw;
            }
        }

        public async Task UpdateOrganisationProfile(UpdateCompanyProfileDto companyProfile)
        {
            var organisation = await _organisationRepository.FindAsync(x => x.Id == companyProfile.Id);
            organisation.SetName(companyProfile.CompanyName);
            organisation.SetEmail(companyProfile.Email);

            if (companyProfile.FileStream != null)
            {
                if (!string.IsNullOrWhiteSpace(organisation.LogoBlobName))
                {
                    var blobDeleted = await _fileAppService.DeleteBlobAsync(organisation.LogoBlobName);
                    _logger.LogInformation($"Attempted to delete Logo blob '{organisation.LogoBlobName}' for user with Id: {companyProfile.Id} with result: {blobDeleted}");
                }

                var blobFileName = RandomNameGenerator.GenerateRandomName(10);
                organisation.LogoBlobName = blobFileName;
                organisation.LogoContentType = companyProfile.LogoContentType;
                organisation.LogoFileName = companyProfile.LogoFileName;

                try
                {
                    await _fileAppService.SaveBlobAsync(new SaveBlobInputDto { Name = blobFileName, Content = ReadFully(companyProfile.FileStream) });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error occured during Logo upload for user {organisation.Id}: {ex.Message}");
                    _logger.LogException(ex);
                }
            }

            await _organisationRepository.UpdateAsync(organisation);
        }

        private static byte[] ReadFully(Stream input)
        {
            using MemoryStream ms = new();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
