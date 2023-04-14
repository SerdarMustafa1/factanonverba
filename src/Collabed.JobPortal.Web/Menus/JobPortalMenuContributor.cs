using Collabed.JobPortal.Localization;
using System.Threading.Tasks;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Collabed.JobPortal.Web.Menus;

public class JobPortalMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<JobPortalResource>();

        context.Menu.AddItem(new ApplicationMenuItem("SocialFeed", displayName: "Social Feed", "https://buildmytalent.com/activity-feed/"));
        context.Menu.AddItem(new ApplicationMenuItem("SocialGroups", displayName: "Social Groups", "https://buildmytalent.com/groups"));
        context.Menu.AddItem(new ApplicationMenuItem("SocialDirectory", displayName: "Social Directory", "https://buildmytalent.com/directory"));
        context.Menu.AddItem(new ApplicationMenuItem("SocialForums", displayName: "Social Forums", "https://buildmytalent.com/forums"));
        context.Menu.AddItem(new ApplicationMenuItem("Companies", displayName: "Companies", "https://buildmytalent.com/employers"));
        context.Menu.AddItem(new ApplicationMenuItem("JobsBoard", displayName: "Jobs Board", "~/jobDashboard"));

        //below adds menu items to display in the middle of navigation bar
        //context.Menu.Items.Insert(
        //    0,
        //    new ApplicationMenuItem(
        //        JobPortalMenus.Home,
        //        l["Menu:Home"],
        //        "~/",
        //        icon: "fas fa-home",
        //        order: 0
        //    )
        //);

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
    }
}
