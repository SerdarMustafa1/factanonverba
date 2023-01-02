using System;
using System.Threading.Tasks;
using Autofac.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Collabed.JobPortal.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddLogging(logBuilder => logBuilder.AddApplicationInsights());

            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac();
            await builder.AddApplicationAsync<JobPortalWebModule>();
            var app = builder.Build();

            await app.InitializeApplicationAsync();
            await app.RunAsync();


            return 0;
        }
        catch (Exception ex)
        {
            return 1;
        }
    }
}
