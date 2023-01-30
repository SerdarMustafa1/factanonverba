using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.BlobStoring.Azure;
using Microsoft.Extensions.DependencyInjection;
using Collabed.JobPortal.Settings;
using Volo.Abp.BlobStoring;
using Microsoft.Extensions.Configuration;
using Collabed.JobPortal.BlobStorage;

namespace Collabed.JobPortal;
[DependsOn(
typeof(JobPortalDomainModule),
typeof(AbpAccountApplicationModule),
    typeof(JobPortalApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
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
        ConfigureBlobStoringOptions(context);
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
            options.Containers.Configure<CvContainer>(container =>
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
