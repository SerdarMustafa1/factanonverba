using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.Permissions;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace Collabed.JobPortal.Web.Pages.Clients
{
    [Authorize(Roles = "admin")]
    public class ClientsModel : AbpPageModel
    {
        private readonly IJobAppService _jobAppService;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IOrganisationRepository _organisationRepository;
        public int TotalCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public List<Client> Clients { get; set; }

        public ClientsModel(IJobAppService jobAppService, IPermissionManager permissionManager, IRepository<IdentityUser, Guid> userRepository, IOrganisationRepository organisationRepository)
        {
            _jobAppService = jobAppService;
            _permissionManager = permissionManager;
            _userRepository = userRepository;
            _organisationRepository = organisationRepository;
        }

        public async Task OnGetAsync()
        {
            await GetClientsAsync(CurrentPage);
        }

        public void OnPostSubmit()
        {
        }

        private async Task GetClientsAsync(int page)
        {
            var organisations = await _organisationRepository.GetPagedListWithDetailsAsync((page - 1)* PageSize, PageSize, sorting: "");
            if (!organisations.Any())
            {
                return;
            }

            Clients = organisations.Select(x => new Client
            {
                CompanyId = x.Id,
                CompanyName = x.Name,
                PermissionStatus = GetPermissionStatus(x.Members),
                Jobs = x.PostedJobs != null && x.PostedJobs.Any() ? x.PostedJobs.Select(x => x.Id).ToList() : new List<Guid>(),
                //Members = x.Members != null && x.Members.Any() ? x.Members.Select(x => x.UserId).ToList() : new List<Guid>()
                UserId = x.Members != null && x.Members.Any() ? x.Members.Select(x => x.UserId).FirstOrDefault() : default
            }).ToList();

            TotalCount = (int)await _organisationRepository.GetCountAsync();
        }

        private bool GetPermissionStatus(ICollection<OrganisationMember> members)
        {
            if (members == null || !members.Any())
                return false;
            // TODO: Once Employer/Employee structure is defined update this logic
            var userId = members.FirstOrDefault().UserId;

            var permissions = _permissionManager.GetAllForUserAsync(userId).Result;

            if (permissions.Any(x => x.Name == BmtPermissions.PostJobs && x.IsGranted))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private async Task GetJobListingsAsync(int page)
        //{
        //    var organisationId = ExtractOrganisationId();

        //    var listings = await _jobAppService.GetListAsync(new JobGetListInput
        //    {
        //        OrganisationId = organisationId,
        //        MaxResultCount =  PageSize,
        //        SkipCount = (page - 1)* PageSize
        //    });

        //    JobListings = listings.Items.Select(x => new JobListing
        //    {
        //        Reference = x.Reference,
        //        Title = x.Title
        //    }).ToList();
        //    TotalCount = (int)listings.TotalCount;
        //}

        private Guid ExtractOrganisationId()
        {
            var organisationClaim = CurrentUser.FindClaim(ClaimNames.OrganisationClaim);
            if (organisationClaim == null || string.IsNullOrEmpty(organisationClaim.Value))
            {
                throw new BusinessException(message: "User is not allowed to post jobs. User is not a member of any organisation.");
            }
            var organisationId = Guid.Parse(organisationClaim.Value);
            return organisationId;
        }
    }

    public class Client
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool PermissionStatus { get; set; }
        public List<Guid> Jobs { get; set; }
        //public List<Guid> Members { get; set; }
        public Guid UserId { get; set; }
    }
}