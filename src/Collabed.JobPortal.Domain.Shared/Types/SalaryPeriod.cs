using System.ComponentModel.DataAnnotations;

namespace Collabed.JobPortal.Types
{
    public enum SalaryPeriod
    {
        [Display(Name = "hour")]
        Hourly,
        [Display(Name = "day")]
        Daily,
        [Display(Name = "week")]
        Weekly,
        [Display(Name = "month")]
        Monthly,
        [Display(Name = "annum")]
        Yearly
    }
}
