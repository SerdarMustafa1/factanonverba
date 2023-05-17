using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Collabed.JobPortal.Web.Pages.JobListings
{
    [IgnoreAntiforgeryToken]
    public class DeleteJobListingModal : AbpPageModel
    {
        public DeleteJobListingModal()
        {

        }
        public string Reference { get; set; }

        public void OnGet(string reference)
        {
            Reference = reference;
        }
        public void OnPost()
        {
        }
    }
}
