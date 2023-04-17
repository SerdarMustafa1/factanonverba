using Collabed.JobPortal.Jobs;
using Volo.Abp.Application.Services;

namespace Collabed.Application.Helpers
{
    public static class SalaryRangeHelper
    {
        public static string GetSalaryRange(float? min, float? max, bool displayEstimated)
        {
            string salaryEstimated = string.Empty;
            if (displayEstimated)
                salaryEstimated = " (estimated)";
            if (min.HasValue && min.Value > 0 && max.HasValue && max.Value == min.Value)
            {
                return $"£{min.Value:N0}{salaryEstimated}";
            }
            else if (min.HasValue && min.Value > 0 && (!max.HasValue || max.Value == 0))
            {
                return $"£{min.Value:N0}{salaryEstimated}";
            }
            else if (max.HasValue && max.Value > 0 && (!min.HasValue || min.Value == 0))
            {
                return $"£{max.Value:N0}{salaryEstimated}";
            }
            else if (min.HasValue && min.Value > 0
                && max.HasValue && max.Value > 0)
            {
                return $"£{min.Value:N0} - £{max.Value:N0}{salaryEstimated}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
