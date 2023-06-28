using AutoMapper;
using Collabed.JobPortal.Account;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Web.Models;
using Collabed.JobPortal.Web.Pages.Applications;
using Collabed.JobPortal.Web.Pages.JobListings;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.Identity.Web.Pages.Identity.Roles;
using CreateUserModalModel = Collabed.JobPortal.Web.Pages.Identity.Users.CreateModalModel;
using EditUserModalModel = Collabed.JobPortal.Web.Pages.Identity.Users.EditModalModel;

namespace Collabed.JobPortal.Web;

public class JobPortalWebAutoMapperProfile : AbpIdentityWebAutoMapperProfile
{
    public JobPortalWebAutoMapperProfile() : base()
    {
        //Define your AutoMapper configuration here for the Web project.
        CreateMap<JobDto, JobBase>();
        CreateMap<ProfileDto, CompanyAccountModel>()
            .MapExtraProperties();
        CreateMap<ProfileDto, IndividualAccountModel>();
        CreateMap<IndividualAccountModel, UpdateUserProfileDto>();
        CreateMap<CompanyAccountModel, UpdateCompanyProfileDto>();
        CreateMap<IdentityUser, ProfileDto>()
            .Ignore(x => x.UserType)
            .MapExtraProperties();
        CreateMap<JobSummaryDto, Listing>();
        CreateMap<JobDto, JobSummary>()
            .ForMember(dest => dest.PositionsAvailable, opt => opt.Ignore());
    }

    protected override void CreateUserMappings()
    {
        //List
        CreateMap<IdentityUserDto, EditUserModalModel.UserInfoViewModel>()
            .MapExtraProperties(mapToRegularProperties: true)
            .Ignore(x => x.Password);

        //CreateModal
        CreateMap<CreateUserModalModel.UserInfoViewModel, IdentityUserCreateDto>()
            .MapExtraProperties(mapToRegularProperties: true)
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        CreateMap<IdentityRoleDto, CreateUserModalModel.AssignedRoleViewModel>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

        //EditModal
        CreateMap<EditUserModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
            .MapExtraProperties(mapToRegularProperties: true)
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        CreateMap<IdentityRoleDto, EditUserModalModel.AssignedRoleViewModel>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());
    }

    protected override void CreateRoleMappings()
    {
        //List
        CreateMap<IdentityRoleDto, EditModalModel.RoleInfoModel>();

        //CreateModal
        CreateMap<CreateModalModel.RoleInfoModel, IdentityRoleCreateDto>()
            .MapExtraProperties(mapToRegularProperties: true);

        //EditModal
        CreateMap<EditModalModel.RoleInfoModel, IdentityRoleUpdateDto>()
            .MapExtraProperties(mapToRegularProperties: true);
    }
}
