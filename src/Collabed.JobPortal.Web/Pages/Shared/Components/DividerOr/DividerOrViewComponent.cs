using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Web.Pages.Shared.Components.DividerOr
{
    public class DividerOrViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Pages/Shared/Components/DividerOr/Default.cshtml");
        }
    }
}
