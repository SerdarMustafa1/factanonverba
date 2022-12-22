using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Collabed.JobPortal.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class JobPortalDbContextFactory : IDesignTimeDbContextFactory<JobPortalDbContext>
{
    public JobPortalDbContext CreateDbContext(string[] args)
    {
        JobPortalEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<JobPortalDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new JobPortalDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Collabed.JobPortal.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
