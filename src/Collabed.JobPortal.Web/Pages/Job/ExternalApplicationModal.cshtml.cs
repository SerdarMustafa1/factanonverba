using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.Job.Apply
{
    public class ExternalApplicationModalModel : AbpPageModel
    {
        public string HiringCompany { get; set; }
        public string JobTitle { get; set; }

        public void OnGet(string hiringCompany, string jobTitle)
        {
            HiringCompany = hiringCompany;
            JobTitle = jobTitle;
        }
    }
}
