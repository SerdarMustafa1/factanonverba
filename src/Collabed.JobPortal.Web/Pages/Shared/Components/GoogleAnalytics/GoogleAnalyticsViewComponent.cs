using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Web.Pages.Shared.Components.GoogleAnalytics
{
    public class GoogleAnalyticsViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Pages/Shared/Components/GoogleAnalytics/Default.cshtml");
        }
    }
}
