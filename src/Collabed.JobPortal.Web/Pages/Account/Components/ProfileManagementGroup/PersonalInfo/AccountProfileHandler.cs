using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using static Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo.AccountProfilePersonalInfoManagementGroupViewComponent;

namespace Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

[IgnoreAntiforgeryToken]
public class AccountProfileHandler : AbpPageModel
{
    public AccountProfileHandler()
    {

    }
    public Task<IActionResult> OnPostAsync(PersonalInfoModel input)
    {
        return null;
    }
}
