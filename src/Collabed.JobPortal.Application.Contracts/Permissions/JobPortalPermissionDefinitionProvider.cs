using Collabed.JobPortal.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Collabed.JobPortal.Permissions;

public class JobPortalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BmtPermissions.GroupName);
        //Define your own permissions here. Example:
        myGroup.AddPermission(BmtPermissions.PostJobs, L("Permission:PostAJob"));
        myGroup.AddPermission(BmtPermissions.ManageJobs, L("Permission:ManageJobs"));
        myGroup.AddPermission(BmtPermissions.ApplyForJobs, L("Permission:ApplyForJobs"));
        myGroup.AddPermission(BmtPermissions.ViewApplicantDashboard, L("Permission:ViewApplicantDashboard"));
        myGroup.AddPermission(BmtPermissions.Admin, L("Permission:Admin"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<JobPortalResource>(name);
    }
}
