using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Threading;

namespace Collabed.JobPortal.Web.Pages.Shared.Components.JobPostNavigation
{
    public class JobPostNavigationViewModel : PageModel
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Value must be between 1 and 5.")]
        public int Step { get; private set; }

        public JobPostNavigationViewModel(int step)
        {
            Step = step;
        }

        public void OnGet()
        {
        }
    }
}
