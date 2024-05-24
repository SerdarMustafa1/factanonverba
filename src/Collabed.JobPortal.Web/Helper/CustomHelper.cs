using Scriban;
using System;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Helper
{
    public static class CustomHelper
    {
        public static int CalculateProgressBar(double totalSteps, double currentStep)
        {
            var progressBarValue = Math.Round(currentStep / totalSteps * 100, 0);

            return (int)progressBarValue;
        }
    }
}
