using Collabed.JobPortal.Account;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.User;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

public partial class AccountProfilePersonalInfoManagementGroupViewComponent : AbpViewComponent
{
    protected IUserProfileAppService ProfileAppService { get; }
    private readonly IBmtAccountAppService _bmtAccountAppService;
    private readonly IOrganisationAppService _organisationAppService;

    public AccountProfilePersonalInfoManagementGroupViewComponent(
        IUserProfileAppService profileAppService, IBmtAccountAppService bmtAccountAppService, IOrganisationAppService organisationAppService)
    {
        ProfileAppService = profileAppService;

        ObjectMapperContext = typeof(AbpAccountWebModule);
        _bmtAccountAppService = bmtAccountAppService;
        _organisationAppService = organisationAppService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await ProfileAppService.GetAsync();


        if (user.UserType == UserType.Candidate)
        {
            var model = ObjectMapper.Map<ProfileDto, PersonalInfoModel>(user);
            var userProfile = await _bmtAccountAppService.GetUserProfileByIdAsync(user.Id);
            model.PostCode = userProfile.PostCode;
            model.CvFileName = userProfile.CvFileName;
            return View("~/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Individual.cshtml", model);
        }
        else
        {
            var model = ObjectMapper.Map<ProfileDto, CompanyInfoModel>(user);
            // TODO: Once organisation structure is defined, replace it to get organisation by id, not owner email
            var orgProfile = await _organisationAppService.GetOrganisationByEmailAsync(user.Email);
            model.LogoFileName = orgProfile.LogoFileName;
            return View("~/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Company.cshtml", model);
        }
    }

    public Task<IActionResult> OnPostAsync(PersonalInfoModel input)
    {
        return null;
    }

    public class CompanyInfoModel : BaseInfoModel
    {
        public string CompanyName { get; set; }
        public string LogoFileName { get; set; }
    }

    public class PersonalInfoModel : BaseInfoModel
    {
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public string CvFileName { get; set; }
        public IFormFile NewCv { get; set; }

        public Task<IActionResult> OnPostAsync(PersonalInfoModel input)
        {
            return null;
        }
    }
}