using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;

namespace Collabed.JobPortal.Jobs
{
    public class AdzunaJobImportWorker : BackgroundWorkerBase
    {
        private const string schedule = "0 1 3 * * ?"; // every day at 01:00
        private readonly CronExpression _cron;
        private readonly IJobAppService _jobAppService;

        public AdzunaJobImportWorker(IJobAppService jobAppService)
        {
            _cron = new CronExpression(schedule);
            _jobAppService=jobAppService;
        }

        public async override Task StartAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var utcNow = DateTime.UtcNow;
                var nextUtc = _cron.GetNextValidTimeAfter(utcNow);
                await Task.Delay(nextUtc.Value - utcNow, cancellationToken);
                await DoWorkAsync();
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Stopped Adzuna Job Import Worker");
        }

        protected async Task DoWorkAsync()
        {
            Logger.LogInformation("Starting: Importing job ads from Adzuna");

            try
            {
                await _jobAppService.FeedAllAdzunaJobsAsync();
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
