using Collabed.JobPortal.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class JobPortalController : AbpControllerBase
{
    protected JobPortalController()
    {
        LocalizationResource = typeof(JobPortalResource);
    }
}
