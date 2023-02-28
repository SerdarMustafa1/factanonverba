using Collabed.JobPortal.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
namespace Collabed.JobPortal;
[DependsOn(
	typeof(JobPortalApplicationContractsModule),
	typeof(AbpAccountHttpApiModule),
	typeof(AbpIdentityHttpApiModule),
	typeof(AbpPermissionManagementHttpApiModule),
	typeof(AbpFeatureManagementHttpApiModule),
	typeof(AbpSettingManagementHttpApiModule)
	)]
public class JobPortalHttpApiModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		ConfigureLocalization();
	}

	public override void PreConfigureServices(ServiceConfigurationContext context)
	{
		PreConfigure<IMvcBuilder>(mvcBuilder =>
		{
			mvcBuilder.AddApplicationPartIfNotExists(typeof(JobPortalHttpApiModule).Assembly);
		});
	}

	private void ConfigureLocalization()
	{
		Configure<AbpLocalizationOptions>(options =>
		{
			options.Resources
				.Get<JobPortalResource>()
				.AddBaseTypes(
					typeof(AbpUiResource)
				);
		});
	}
}
