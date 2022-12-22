using Collabed.JobPortal.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class JobPortalPageModel : AbpPageModel
{
    protected JobPortalPageModel()
    {
        LocalizationResourceType = typeof(JobPortalResource);
    }
}
