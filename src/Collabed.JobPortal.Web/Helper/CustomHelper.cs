using Scriban;
using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace Collabed.JobPortal.Web.Helper
{
    public static class CustomHelper
    {
        //TODO - Simplify this method
        public static int CalculateProgressBar(double totalSteps, double currentStep)
        {
            int firstStepPercentage = 0;
            if (totalSteps <= 3)
            {
                firstStepPercentage = (int)(100 - Math.Round(1 / totalSteps * 100, 0));

                if (currentStep == 1)
                    return firstStepPercentage;
                else
                {
                    int value;
                    if (currentStep == 2)
                    {
                        value = (int)(100 - Math.Round(currentStep / totalSteps * 100, 0));
                        
                        return firstStepPercentage + value/2;
                    }

                    if(currentStep >= 3)
                    {
                        value = 100;
                        return value;
                    }
                }
            }
            else
            {
                var val = 0;
                if(totalSteps == 4)
                {
                    firstStepPercentage = 60;

                    if (currentStep == 1)
                        return firstStepPercentage;
                    if(currentStep == 2)
                    {
                        val = (int)(Math.Round(currentStep / totalSteps * 100, 0));
                        return firstStepPercentage + val/2;
                    }
                    if(currentStep == 3)
                    {
                        val = 100 - (int)(Math.Round(currentStep / totalSteps * 100, 0));
                        return firstStepPercentage + val;
                    }
                    if(currentStep == 4)
                    {
                        return 100;
                    }
                }

                if(totalSteps == 5 || totalSteps == 6)
                {
                    firstStepPercentage = 45;

                    if (currentStep == 1)
                        return firstStepPercentage;

                    if(currentStep == 2)
                    {
                        val = (int)(Math.Round(currentStep / totalSteps * 100, 0));
                        return firstStepPercentage + val / 2;
                    }

                    if(currentStep == 3)
                    {
                        val = 100 - (int)(Math.Round(currentStep / totalSteps * 100, 0));
                        return firstStepPercentage + val/2;
                    }

                    if(currentStep == 4)
                    {
                        val = 100 - (int)(Math.Round(currentStep / totalSteps * 100, 0));
                        var tot = firstStepPercentage + val;
                        if(totalSteps == 5)
                        {
                            return 85;
                        }
                        else
                            return tot;
                    }

                    if(totalSteps == 5 && currentStep == 5)
                    {
                        return 100;
                    }
                    else
                    {
                        if(totalSteps == 6)
                        {
                            if(currentStep == 5)
                            {
                                val = 100 - (int)(Math.Round(currentStep / totalSteps * 100, 0));
                                return 85;
                            }

                            if(currentStep == totalSteps)
                            {
                                return 100;
                            }
                        }
                    }
                }
            }
            return firstStepPercentage;
        }
    }
}
