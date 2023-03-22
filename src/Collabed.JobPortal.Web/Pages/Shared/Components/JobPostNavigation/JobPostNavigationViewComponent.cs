using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Web.Pages.Shared.Components.JobPostNavigation
{
    public class JobPostNavigationViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int step)
        {
            return View(new JobPostNavigationViewModel(step));
        }
    }
}
