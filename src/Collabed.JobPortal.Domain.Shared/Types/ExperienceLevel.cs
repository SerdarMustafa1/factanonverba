using System.ComponentModel.DataAnnotations;

namespace Collabed.JobPortal.Types
{
    public enum ExperienceLevel
    {
        Internship,
        [Display(Name = "Entry Level")]
        EntryLevel,
        Associate,
        [Display(Name = "Mid-Senior Level")]
        MidSeniorLevel,
        Director,
        Executive
    }
}
