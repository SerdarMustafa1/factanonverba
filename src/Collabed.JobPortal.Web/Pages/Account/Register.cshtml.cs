using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Collabed.JobPortal.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public BMTRegisterModel(IAccountAppService accountAppService, EmailService emailService) : base(accountAppService)
    {
        AccountAppService = accountAppService;
        _emailService = emailService;
    }
    protected override async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string emailAddress)
    {
        await IdentityOptions.SetAsync();
        // HACK: Below new IdentityUser is being created, second argument stands for the UserName
        var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);
        user.IsExternal = true;

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

    protected override async Task RegisterLocalUserAsync()
    {
        await _emailService.SendEmailAsync(Input.EmailAddress, EmailTemplates.RegistrationSubjectTemplate, EmailTemplates.RegistrationBodyTemplate);
        await base.RegisterLocalUserAsync();
    }
}
