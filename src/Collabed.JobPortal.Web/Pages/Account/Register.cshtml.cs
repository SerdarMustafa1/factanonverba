using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Collabed.JobPortal.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Collabed.JobPortal.Web.Pages.Account;

public class BMTRegisterModel : RegisterModel
{
    private readonly EmailService _emailService;

    [BindProperty]
    public new BMTPostInput Input { get; set; }
    public BMTRegisterModel(IAccountAppService accountAppService, EmailService emailService) : base(accountAppService)
    {
        AccountAppService = accountAppService;
        _emailService = emailService;
    }


    public override async Task<IActionResult> OnGetAsync()
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
            var nameClaim = identity.FindFirst(ClaimTypes.Name);
            if (emailClaim == null && nameClaim == null)
            {
                return; // HACK This comes from Volo.ABP, it should be re-done to give any info about the bug.
            }

            Input = new BMTPostInput { EmailAddress = emailClaim?.Value, FullName = nameClaim?.Value };
        }
    }

    public override async Task<IActionResult> OnPostAsync()
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

                await RegisterExternalUserAsync(externalLoginInfo, Input.EmailAddress);
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
    }

    protected override async Task RegisterLocalUserAsync()
    {
        ValidateModel();

        var userDto = await AccountAppService.RegisterAsync(
            new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                UserName = Input.UserName
            }
        );

        var user = await UserManager.GetByIdAsync(userDto.Id);
        await SignInManager.SignInAsync(user, isPersistent: true);
    }

    protected override async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string emailAddress)
    {
        await IdentityOptions.SetAsync();
        // HACK: Below new IdentityUser is being created, second argument stands for the UserName
        var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);
        user.IsExternal = true;

        user.Name = Input.FullName;
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

    protected override async Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
            !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
        }
    }

    public class BMTPostInput : PostInput
    {
        [Required]
        public string FullName { get; set; }
    }
}
