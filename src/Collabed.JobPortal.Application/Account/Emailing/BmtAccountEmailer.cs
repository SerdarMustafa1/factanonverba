using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.Email;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Account.Emailing.Templates;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;

namespace Collabed.JobPortal.Account.Emailing
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountEmailer), typeof(IBmtAccountEmailer))]
    public class BmtAccountEmailer : AccountEmailer, IBmtAccountEmailer
    {
        public BmtAccountEmailer(
            IEmailSender emailSender,
            ITemplateRenderer templateRenderer,
            IStringLocalizer<AccountResource> stringLocalizer,
            IAppUrlProvider appUrlProvider,
            ICurrentTenant currentTenant) : base(emailSender, templateRenderer, stringLocalizer, appUrlProvider, currentTenant)
        {
        }

        public async Task SendEmailVerificationRequestAsync(IdentityUser user, string callbackUrl)
        {
            var emailContent = await TemplateRenderer.RenderAsync(
                BmtAccountEmailTemplates.EmailVerification, new { callbackUrl });

            await EmailSender.SendAsync(user.Email, EmailTemplates.ConfirmEmailSubject, emailContent);
        }

        public override async Task SendPasswordResetLinkAsync(
        IdentityUser user,
        string resetToken,
        string appName,
        string returnUrl = null,
        string returnUrlHash = null)
        {
            Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

            var url = await AppUrlProvider.GetResetPasswordUrlAsync(appName);

            //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
            var link = $"{url}?userId={user.Id}&{TenantResolverConsts.DefaultTenantKey}={user.TenantId}&resetToken={UrlEncoder.Default.Encode(resetToken)}";

            if (!returnUrl.IsNullOrEmpty())
            {
                link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
            }

            if (!returnUrlHash.IsNullOrEmpty())
            {
                link += "&returnUrlHash=" + returnUrlHash;
            }

            var emailContent = await TemplateRenderer.RenderAsync(
                AccountEmailTemplates.PasswordResetLink,
                new { link = link, email = user.Email }
            );

            await EmailSender.SendAsync(
                user.Email,
                StringLocalizer["PasswordReset"],
                emailContent
            );
        }
    }
}
