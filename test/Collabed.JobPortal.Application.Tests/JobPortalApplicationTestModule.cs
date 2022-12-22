using Volo.Abp.Modularity;

namespace Collabed.JobPortal;

[DependsOn(
    typeof(JobPortalApplicationModule),
    typeof(JobPortalDomainTestModule)
    )]
public class JobPortalApplicationTestModule : AbpModule
{

}
