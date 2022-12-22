using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Collabed.JobPortal;

public class JobPortalWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<JobPortalWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
