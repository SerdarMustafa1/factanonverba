using Collabed.JobPortal.Roles;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace Collabed.JobPortal.EntityFrameworkCore
{
    public class RolesDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<IdentityRole, Guid> _identityRoleRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IdentityRoleManager _identityRoleManager;
        private readonly IGuidGenerator GuidGenerator;

        // Permissions
        public const string GroupName = "BMT";
        public const string Admin = GroupName + ".Admin";
        public const string ManageJobsPermission = GroupName + ".ManageJobs";
        public const string PostJobPermission = GroupName + ".PostAJob";
        public const string ApplyForJobsPermission = GroupName + ".ApplyForJobs";
        public const string ViewApplicantDashboardPermission = GroupName+ ".ViewApplicantDashboard";
        public const string AdminPermission = GroupName + ".Admin";

        public RolesDataSeedContributor(IRepository<IdentityRole, Guid> identityRoleRepository, IPermissionManager permissionManager, IdentityRoleManager identityRoleManager, IGuidGenerator guidGenerator)
        {
            _identityRoleRepository = identityRoleRepository;
            _permissionManager = permissionManager;
            _identityRoleManager = identityRoleManager;
            GuidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            //await _permissionManager.SetForRoleAsync(RoleNames.Admin, AdminPermission, true);

            if (await _identityRoleRepository.CountAsync() > 1)
            {
                return;
            }


            var organisationOwnerRole = new IdentityRole(GuidGenerator.Create(),
                RoleNames.OrganisationOwnerRole)
            {
                IsStatic = true,
                IsPublic = true
            };

            await _identityRoleManager.CreateAsync(organisationOwnerRole);
            await _permissionManager.SetForRoleAsync(RoleNames.OrganisationOwnerRole, ManageJobsPermission, true);

            var applicantRole = new IdentityRole(GuidGenerator.Create(),
                RoleNames.BmtApplicant)
            {
                IsStatic = true,
                IsPublic = true
            };

            await _identityRoleManager.CreateAsync(applicantRole);
            await _permissionManager.SetForRoleAsync(RoleNames.BmtApplicant, ApplyForJobsPermission, true);
            await _permissionManager.SetForRoleAsync(RoleNames.BmtApplicant, ViewApplicantDashboardPermission, true);
        }
    }
}
