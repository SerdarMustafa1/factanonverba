using Collabed.JobPortal.Account;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.User;
using Collabed.JobPortal.Users;
using Collabed.JobPortal.Web.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Account;

public class ManageModel : Volo.Abp.Account.Web.Pages.Account.AccountPageModel
{
    protected IUserProfileAppService ProfileAppService { get; }
    private readonly IBmtAccountAppService BmtAccountAppService;
    private readonly IOrganisationAppService OrganisationAppService;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }
    [BindProperty]
    public IndividualAccountModel IndividualAccountModel { get; set; }
    [BindProperty]
    public CompanyAccountModel CompanyAccountModel { get; set; }
    [HiddenInput]
    [BindProperty]
    public UserType UserType { get; set; }

    public ManageModel(IUserProfileAppService profileAppService, IBmtAccountAppService bmtAccountAppService, IOrganisationAppService organisationAppService)
    {
        ProfileAppService=profileAppService;
        BmtAccountAppService=bmtAccountAppService;
        OrganisationAppService=organisationAppService;
    }

    public async Task<UserType> GetUserType()
    {
        var user = await ProfileAppService.GetAsync();
        return user.UserType;
    }

    public async Task OnGetAsync()
    {
        var user = await ProfileAppService.GetAsync();
        UserType = user.UserType;
        if (user.UserType == UserType.Candidate)
        {
            IndividualAccountModel = ObjectMapper.Map<ProfileDto, IndividualAccountModel>(user);
            var userProfile = await BmtAccountAppService.GetUserProfileByIdAsync(user.Id);
            IndividualAccountModel.PostCode = userProfile?.PostCode;
            IndividualAccountModel.CvFileName = userProfile?.CvFileName;
        }
        else
        {
            CompanyAccountModel = ObjectMapper.Map<ProfileDto, CompanyAccountModel>(user);
            // TODO: Once organisation structure is defined, replace it to get organisation by id, not owner email
            var orgProfile = await OrganisationAppService.GetOrganisationByEmailAsync(user.Email);
            CompanyAccountModel.CompanyName = orgProfile.Name;
            CompanyAccountModel.LogoFileName = orgProfile.LogoFileName;
            CompanyAccountModel.Id= orgProfile.Id;
        }

        if (ReturnUrl != null)
        {
            if (!Url.IsLocalUrl(ReturnUrl) &&
                !ReturnUrl.StartsWith(UriHelper.BuildAbsolute(Request.Scheme, Request.Host, Request.PathBase).RemovePostFix("/")) &&
                !AppUrlProvider.IsRedirectAllowedUrl(ReturnUrl))
            {
                ReturnUrl = null;
            }
        }
    }

    public async Task<IActionResult> OnPostIndividualProfileAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var updateDto = ObjectMapper.Map<IndividualAccountModel, UpdateUserProfileDto>(IndividualAccountModel);
        updateDto.UserName = updateDto.Email;
        await ProfileAppService.UpdateAsync(updateDto);
        await BmtAccountAppService.UpdateUserProfileAsync(updateDto);
        if (IndividualAccountModel.NewCv != null)
        {
            var cvFileName = IndividualAccountModel.NewCv.FileName;
            var cvContentType = IndividualAccountModel.NewCv.ContentType;
            using var memoryStream = new CustomMemoryStream();
            await IndividualAccountModel.NewCv.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            await BmtAccountAppService.UploadCvToUserProfile(updateDto.Id, memoryStream, cvFileName, cvContentType);
            IndividualAccountModel.CvFileName = cvFileName;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostCompanyProfileAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var updateDto = ObjectMapper.Map<CompanyAccountModel, UpdateCompanyProfileDto>(CompanyAccountModel);
        updateDto.UserName = updateDto.Email;
        await ProfileAppService.UpdateAsync(updateDto);
        if (CompanyAccountModel.NewLogo == null)
        {
            await OrganisationAppService.UpdateOrganisationProfile(updateDto);
            return Page();
        }

        updateDto.LogoFileName = CompanyAccountModel.NewLogo.FileName;
        updateDto.LogoContentType = CompanyAccountModel.NewLogo.ContentType;
        using (var memoryStream = new CustomMemoryStream())
        {
            await CompanyAccountModel.NewLogo.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            updateDto.FileStream = memoryStream;
            await OrganisationAppService.UpdateOrganisationProfile(updateDto);
        };
        CompanyAccountModel.LogoFileName = updateDto.LogoFileName;
        UserType = UserType.Organisation;

        return Page();
    }
}
