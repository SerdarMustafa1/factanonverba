using Volo.Abp.Account.Emailing.Templates;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace Collabed.JobPortal.Account.Emailing.Templates
{
    public class BmtAccountEmailTemplateDefinitionProvider : TemplateDefinitionProvider, ITransientDependency
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            var emailLayoutTemplate = context.GetOrNull(AccountEmailTemplates.PasswordResetLink);
            emailLayoutTemplate.WithVirtualFilePath("/Account/Emailing/Templates/BmtPasswordResetLink.tpl", true);

            context.Add(
                new TemplateDefinition(
                    BmtAccountEmailTemplates.EmailVerification,
                    displayName: LocalizableString.Create<AccountResource>($"TextTemplate:ConfirmEmail"),
                    layout: StandardEmailTemplates.Layout,
                    localizationResource: typeof(AccountResource)
                ).WithVirtualFilePath("/Account/Emailing/Templates/ConfirmEmail.tpl", true)
            //Account\Emailing\Templates\
            );
            context.Add(
                new TemplateDefinition(
                    BmtAccountEmailTemplates.ThirdPartyApplication,
                    layout: StandardEmailTemplates.Layout,
                    localizationResource: typeof(AccountResource)
                ).WithVirtualFilePath("/Account/Emailing/Templates/ThirdPartyApplication.tpl", true)
            );
        }
    }
}
