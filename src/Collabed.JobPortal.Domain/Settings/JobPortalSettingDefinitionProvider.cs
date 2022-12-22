using Volo.Abp.Settings;

namespace Collabed.JobPortal.Settings;

public class JobPortalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(JobPortalSettings.MySetting1));
    }
}
