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
        context.Menu.AddItem(new ApplicationMenuItem("Groups", displayName: "Groups", "https://buildmytalent.com/groups"));
        context.Menu.AddItem(new ApplicationMenuItem("Directory", displayName: "Directory", "https://buildmytalent.com/directory"));
        context.Menu.AddItem(new ApplicationMenuItem("Forums", displayName: "Forums", "https://buildmytalent.com/forums"));
        context.Menu.AddItem(new ApplicationMenuItem("Employers", displayName: "Employers", "https://buildmytalent.com/employers"));
        context.Menu.AddItem(new ApplicationMenuItem("JobsBoard", displayName: "Job Board", "~/jobDashboard", cssClass: "bold"));

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
