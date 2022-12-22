using Collabed.JobPortal.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Collabed.JobPortal.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(JobPortalEntityFrameworkCoreModule),
    typeof(JobPortalApplicationContractsModule)
    )]
public class JobPortalDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
