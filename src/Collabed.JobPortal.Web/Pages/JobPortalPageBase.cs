using Collabed.JobPortal.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;

namespace Collabed.JobPortal.Web.Pages
{
    public class JobPortalPageBase : Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        [RazorInject] public IStringLocalizer<JobPortalResource> L { get; set; }

        [RazorInject] public IPageLayout PageLayout { get; set; }

        public override Task ExecuteAsync()
        {
            return Task.CompletedTask; // Will be overriden by razor pages. (.cshtml)
        }
    }
}
