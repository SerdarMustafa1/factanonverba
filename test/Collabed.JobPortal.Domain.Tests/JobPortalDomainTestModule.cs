using Collabed.JobPortal.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Collabed.JobPortal;

[DependsOn(
    typeof(JobPortalEntityFrameworkCoreTestModule)
    )]
public class JobPortalDomainTestModule : AbpModule
{

}
