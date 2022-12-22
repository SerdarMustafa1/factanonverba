using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Collabed.JobPortal.Data;
using Volo.Abp.DependencyInjection;

namespace Collabed.JobPortal.EntityFrameworkCore;

public class EntityFrameworkCoreJobPortalDbSchemaMigrator
    : IJobPortalDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreJobPortalDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the JobPortalDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<JobPortalDbContext>()
            .Database
            .MigrateAsync();
    }
}
