using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.PayPal;
using Collabed.JobPortal.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Azure;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.VirtualFileSystem;

namespace Collabed.JobPortal;
[DependsOn(
typeof(JobPortalDomainModule),
typeof(AbpAccountApplicationModule),
    typeof(JobPortalApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpTextTemplatingScribanModule)
    )]
[DependsOn(typeof(AbpBlobStoringAzureModule))]
public class JobPortalApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<JobPortalApplicationModule>();
        });
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<JobPortalApplicationModule>();
        });
        ConfigureBlobStoringOptions(context);

        context.Services.AddTransient(provider =>
        {
            var options = provider.GetService<IOptions<PayPalOptions>>().Value;

            if (options.Environment.IsNullOrWhiteSpace() || options.Environment == PayPalConsts.Environment.Sandbox)
            {
                return new PayPalHttpClient(new SandboxEnvironment(options.ClientId, options.Secret));
            }

            return new PayPalHttpClient(new LiveEnvironment(options.ClientId, options.Secret));
        });

        var configuration = context.Services.GetConfiguration();
        Configure<BroadbeanOptions>(configuration.GetSection("ExternalFeed:Broadbean"));
        Configure<IdibuOptions>(configuration.GetSection("ExternalFeed:Idibu"));
        Configure<MapsSearchOptions>(options =>
        {
            options.SubscriptionKey =
            configuration["Settings:MapsSubscriptionKey"];
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<NightBackgroundWorker>();
    }

    private void ConfigureBlobStoringOptions(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var blobStorageConfigSection = configuration.GetSection(SettingsConsts.AzureBlobStorageSectionName);
        var blobStorageConfig = blobStorageConfigSection.Get<BlobStorageOptions>();

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseAzure(azure =>
                {
                    azure.ConnectionString = blobStorageConfig?.ConnectionString;
                    azure.ContainerName = blobStorageConfig?.DefaultContainerName;
                    azure.CreateContainerIfNotExists = true;
                });
            });
            options.Containers.Configure<JobPortalContainer>(container =>
            {
                container.UseAzure(azure =>
                {
                    azure.ConnectionString = blobStorageConfig?.ConnectionString;
                    azure.CreateContainerIfNotExists = true;
                });
            });
        });
    }
}
