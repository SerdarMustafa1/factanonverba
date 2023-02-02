using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Collabed.JobPortal.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUglify.Helpers;
using NUglify.JavaScript.Syntax;
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

public class BMTRegisterModel : AccountPageModel
{
    private readonly EmailService _emailService;
    public ExternalProviderModel[] ExternalProviders { get; private protected set; }
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }
    [BindProperty(SupportsGet = true)]
    public bool IsExternalLogin { get; set; }
    [BindProperty(SupportsGet = true)]
    public string ExternalLoginAuthSchema { get; set; }
    [BindProperty]
    public BMTPostInput RegisterPostInput { get; set; }

    public BMTRegisterModel(IAccountAppService accountAppService, EmailService emailService)
    {
        AccountAppService = accountAppService;
        _emailService = emailService;
        var liProvider = new ExternalProviderModel() { DisplayName = "LinkedIn", AuthenticationScheme = "LinkedIn" };
        var indeedProvider = new ExternalProviderModel() { DisplayName = "Indeed", AuthenticationScheme = "Indeed" };
        var externalProviders = new ExternalProviderModel[] { liProvider, indeedProvider };
        this.ExternalProviders = externalProviders;
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

            RegisterPostInput = new BMTPostInput { EmailAddress = emailClaim?.Value, FirstName = givenName?.Value, LastName = surname?.Value };
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await CheckSelfRegistrationAsync();

            if (IsExternalLogin)
            {
                var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    Logger.LogWarning("External login info is not available");
                    return RedirectToPage("./Login");
                }

                await RegisterExternalUserAsync(externalLoginInfo, RegisterPostInput.EmailAddress);
            }
            else
            {
                await RegisterLocalUserAsync();
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

    protected async Task RegisterLocalUserAsync()
    {
        ValidateModel();
        var userDto = await AccountAppService.RegisterAsync(
            new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = RegisterPostInput.EmailAddress,
                Password = RegisterPostInput.Password,
                UserName = RegisterPostInput.UserName
            }
        );

        var user = await UserManager.GetByIdAsync(userDto.Id);
        await SignInManager.SignInAsync(user, isPersistent: true);
        await _emailService.SendEmailAsync(RegisterPostInput.EmailAddress, EmailTemplates.RegistrationSubjectTemplate, EmailTemplates.RegistrationBodyTemplate);
    }

    protected async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string emailAddress)
    {
        await IdentityOptions.SetAsync();
        // HACK: Below new IdentityUser is being created, second argument stands for the UserName
        var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);
        user.IsExternal = true;

        user.Name = RegisterPostInput.FirstName;
        user.Surname = RegisterPostInput.LastName;
        System.Console.WriteLine(user.Tokens); 
        (await UserManager.CreateAsync(user)).CheckErrors();
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

    protected async Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
            !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
        }
    }

    public class BMTPostInput
    {
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
        [RegularExpression(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", ErrorMessage = "Please type valid email address")]
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
}
