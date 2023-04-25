using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Collabed.JobPortal.Jobs
{
    public class AdzunaJobImportWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IJobAppService _jobAppService;

        public AdzunaJobImportWorker(IJobAppService jobAppService, AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            _jobAppService = jobAppService;
            Timer.Period = 3540000; //59 minutes
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Starting: Importing job ads from Adzuna");

            var start = new TimeSpan(3, 0, 0); //3.01 o'clock
            var end = new TimeSpan(4, 0, 0); //4.01 o'clock
            var now = DateTime.Now.TimeOfDay;

            if ((now < start) || (now > end))
            {
                return;
            }

            var jobAppService = workerContext
                .ServiceProvider
                .GetRequiredService<IJobAppService>();

            try
            {
                await jobAppService.FeedAllAdzunaJobsAsync();
                Logger.LogInformation("Completed: Finished job ads import from Adzuna");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error occured during Adzuna import: {ex.Message}");
                Logger.LogException(ex);
            }
        }
    }
}
