using System.Threading.Tasks;

namespace Collabed.JobPortal.Data;

public interface IJobPortalDbSchemaMigrator
{
    Task MigrateAsync();
}
