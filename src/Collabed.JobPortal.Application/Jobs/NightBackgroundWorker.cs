using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Collabed.JobPortal.Jobs
{
    public class NightBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public NightBackgroundWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            Timer.Period = 3540000; //59 minutes
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var start = new TimeSpan(3, 0, 0); //3.01 o'clock
            var end = new TimeSpan(4, 0, 0); //4.01 o'clock
            var now = DateTime.Now.TimeOfDay;
            
            if ((now < start) || (now > end))
            {
                return;
            }

            Logger.LogInformation("Started night worker job");
            var jobAppService = workerContext
                .ServiceProvider
                .GetRequiredService<IJobAppService>();

            try
            {
                Logger.LogInformation("Starting: Importing job ads from Adzuna");
                await jobAppService.FeedAllAdzunaJobsAsync();
                Logger.LogInformation("Completed: Finished job ads import from Adzuna");
                await jobAppService.ReviewJobsAsync();
                Logger.LogInformation("Completed: Finished updating job statuses");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error occured: {ex.Message}");
                Logger.LogException(ex);
            }
        }
    }
}
