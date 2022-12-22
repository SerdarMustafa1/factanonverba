using Collabed.JobPortal.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Collabed.JobPortal.Permissions;

public class JobPortalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(JobPortalPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(JobPortalPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<JobPortalResource>(name);
    }
}
