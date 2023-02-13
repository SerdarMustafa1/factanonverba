using Collabed.JobPortal.Email;
using Collabed.JobPortal.Extensions;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.Roles;
using Collabed.JobPortal.User;
using Collabed.JobPortal.Users;
using Collabed.JobPortal.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUglify.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using static Volo.Abp.Account.Web.Pages.Account.LoginModel;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Collabed.JobPortal.Web.Pages.Account;

[BindProperties(SupportsGet = true)]
public class BMTRegisterModel : AccountPageModel
{
    private readonly EmailService _emailService;
    private readonly IOrganisationAppService _organisationAppService;
    public ExternalProviderModel[] ExternalProviders { get; private protected set; }

    public string ReturnUrl { get; set; }

    public string ReturnUrlHash { get; set; }

    public bool IsExternalLogin { get; set; }

    public string ExternalLoginAuthSchema { get; set; }

    public BMTRegisterModel(IAccountAppService accountAppService, EmailService emailService, IOrganisationAppService organisationAppService)
    {
        AccountAppService = accountAppService;
        _emailService = emailService;
        var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
        var indeedProvider = new ExternalProviderModel() { DisplayName = "Indeed", AuthenticationScheme = "Indeed" };
        var externalProviders = new ExternalProviderModel[] { liProvider, indeedProvider };
        this.ExternalProviders = externalProviders;
        _organisationAppService = organisationAppService;
    }


    public async Task<IActionResult> OnGetAsync()
    {
        await CheckSelfRegistrationAsync();
        await TrySetEmailAndName();
        return Page();
    }

    private async Task TrySetEmailAndName()
    {
        if (IsExternalLogin)
        {
            var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return; // HACK This comes from Volo.ABP, it should be re-done to give any info about the bug.
            }

            if (!externalLoginInfo.Principal.Identities.Any())
            {
                return; // HACK This comes from Volo.ABP, it should be re-done to give any info about the bug.
            }

            var identity = externalLoginInfo.Principal.Identities.First();
            var emailClaim = identity.FindFirst(ClaimTypes.Email);
            var givenName = identity.FindFirst(ClaimTypes.GivenName);
            var surname = identity.FindFirst(ClaimTypes.Surname);
            if (emailClaim == null && givenName == null)
            {
                return; // HACK This comes from Volo.ABP, it should be re-done to give any info about the bug.
            }

            EmailAddress = emailClaim?.Value;
            FirstName = givenName?.Value;
            LastName = surname?.Value;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var form = Request.Form;
            await CheckSelfRegistrationAsync();

            // TODO: Get user type from the flow
            var userType = UserType.Candidate;
            if (IsExternalLogin)
            {
                var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    Logger.LogWarning("External login info is not available");
                    return RedirectToPage("./Login");
                }

                await RegisterExternalUserAsync(externalLoginInfo, EmailAddress, userType);
            }
            else
            {
                await RegisterLocalUserAsync(userType);
            }

            if (UserManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = EmailAddress, returnUrl = ReturnUrl });
            }

            return Redirect(ReturnUrl ?? "~/");
        }
        catch (BusinessException e)
        {
            Alerts.Danger(GetLocalizeExceptionMessage(e));
            return Page();
        }
        catch (AbpValidationException validationException)
        {
            Logger.LogException(validationException);
            if (validationException != null && validationException.ValidationErrors.Count > 0)
                validationException.ValidationErrors.ForEach(ex =>
                    Logger.LogError($"{ex.ErrorMessage}"));
            return Page();
        }
    }

    protected async Task RegisterLocalUserAsync(UserType userType)
    {
        ValidateModel();

        var registerDto = new RegisterDto
        {
            AppName = "MVC",
            EmailAddress = EmailAddress,
            Password = Password,
            UserName = UserName
        };

        registerDto.SetUserType(userType);

        var userDto = await AccountAppService.RegisterAsync(registerDto);
        var user = await UserManager.GetByIdAsync(userDto.Id);
        user.Name = FirstName;
        user.Surname = LastName;
        await UserManager.UpdateAsync(user);

        // Post MVP: To be moved to a separate flow
        if (userType == UserType.Organisation)
        {
            //TODO: Get Organisation details from ViewModel
            var organisationName = "Organisation xyz";
            await CreateOrganisationAsync(user, organisationName);
        }

        await AssignDefaultRoles(userType, user);

        await SignInManager.SignInAsync(user, isPersistent: true);

        // Send user an email to confirm email address      
        await SendEmailToAskForEmailConfirmationAsync(user);
    }

    private async Task SendEmailToAskForEmailConfirmationAsync(IdentityUser user)
    {
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { userId = user.Id, code = code }, protocol: Request.Scheme);

        var confirmEmailTemplate = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices,
            $"~/Views/Emails/ConfirmEmailTemplate.cshtml",
            new EmailTemplatesModel(callbackUrl));

        await _emailService.SendEmailAsync(EmailAddress, EmailTemplates.ConfirmEmailSubject, confirmEmailTemplate);

    }

    private async Task CreateOrganisationAsync(IdentityUser user, string organisationName)
    {
        var organisationDto = await _organisationAppService.CreateAsync(new CreateOrganisationDto
        {
            Email = user.Email,
            Name = organisationName,
            OwnerId = user.Id
        });

        await UserManager.AddClaimAsync(user, new Claim(ClaimNames.OrganisationClaim, organisationDto.Id.ToString()));
    }

    protected async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string emailAddress, UserType userType)
    {
        await IdentityOptions.SetAsync();
        // HACK: Below new IdentityUser is being created, second argument stands for the UserName
        var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);
        user.IsExternal = true;

        user.Name = FirstName;
        user.Surname = LastName;

        // Skip email confirmation for external users
        user.SetEmailConfirmed(true);

        user.SetUserType(userType);
        user.SetFirstName(FirstName);
        user.SetLastName(LastName);

        System.Console.WriteLine(user.Tokens);
        (await UserManager.CreateAsync(user)).CheckErrors();

        // Post MVP: To be moved to a separate flow
        if (userType == UserType.Organisation)
        {
            //TODO: Get Organisation details from ViewModel
            var organisationName = "Organisation xyz ext";
            await CreateOrganisationAsync(user, organisationName);
            await UserManager.AddToRoleAsync(user, RoleNames.OrganisationOwnerRole);
        }
        if (userType == UserType.Candidate)
        {
            await UserManager.AddToRoleAsync(user, RoleNames.BmtApplicant);
        }
        await AssignDefaultRoles(userType, user);

        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

        var userLoginAlreadyExists = user.Logins.Any(x =>
            x.TenantId == user.TenantId &&
            x.LoginProvider == externalLoginInfo.LoginProvider &&
            x.ProviderKey == externalLoginInfo.ProviderKey);

        if (!userLoginAlreadyExists)
        {
            (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey,
                externalLoginInfo.ProviderDisplayName
            ))).CheckErrors();
        }

        await SignInManager.SignInAsync(user, isPersistent: true);
    }

    private async Task AssignDefaultRoles(UserType userType, IdentityUser user)
    {
        if (userType == UserType.Organisation)
        {
            await UserManager.AddToRoleAsync(user, RoleNames.OrganisationOwnerRole);
        }
        if (userType == UserType.Candidate)
        {
            await UserManager.AddToRoleAsync(user, RoleNames.BmtApplicant);
        }
    }

    protected async Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
            !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
        }
    }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please type your email address")]
    [ExtendedEmailAddress("Please type valid email address")]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = " ")]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(?=.*\d).{8,}$", ErrorMessage = " ")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Your passwords don't match")]
    [DisplayName("Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match each other")]
    [DisableAuditing]
    public string ConfirmPassword { get; set; }

}
