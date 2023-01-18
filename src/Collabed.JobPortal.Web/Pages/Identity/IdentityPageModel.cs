using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity.Web;

namespace Collabed.JobPortal.Web.Pages.Identity;

public abstract class IdentityPageModel : AbpPageModel
{
    protected IdentityPageModel()
    {
        ObjectMapperContext = typeof(AbpIdentityWebModule);
    }
}
