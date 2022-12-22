using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Collabed.JobPortal.Data;

/* This is used if database provider does't define
 * IJobPortalDbSchemaMigrator implementation.
 */
public class NullJobPortalDbSchemaMigrator : IJobPortalDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
