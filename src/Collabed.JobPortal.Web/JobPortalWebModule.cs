using BuildMyTalentOAuthExtensions;
using Collabed.JobPortal.EntityFrameworkCore;
using Collabed.JobPortal.Localization;
using Collabed.JobPortal.Settings;
using Collabed.JobPortal.Web.Menus;
using Collabed.JobPortal.Web.Pages.Shared.Components.Footer;
using Collabed.JobPortal.Web.Pages.Shared.Components.GoogleAnalytics;
using Collabed.JobPortal.Web.Pages.Shared.Components.Spacer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System.IO;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Collabed.JobPortal.Web;

[DependsOn(
    typeof(JobPortalHttpApiModule),
    typeof(JobPortalApplicationModule),
    typeof(JobPortalEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.AbpAspNetCoreMvcUiBasicThemeModule)
    )]
[DependsOn(typeof(AbpIdentityApplicationModule))]
public class JobPortalWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(JobPortalResource),
                typeof(JobPortalDomainModule).Assembly,
                typeof(JobPortalDomainSharedModule).Assembly,
                typeof(JobPortalApplicationModule).Assembly,
                typeof(JobPortalApplicationContractsModule).Assembly,
                typeof(JobPortalWebModule).Assembly
            );
        });

        ConfigureOpenIdDict(context);
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("JobPortal");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureHooks(configuration);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureLocalizationServices();
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureIdentityOptions(context);
        ConfigureSwaggerServices(context.Services);
        Configure<AbpLayoutHookOptions>(options =>
        {
            options.Add(
                LayoutHooks.Head.Last,
                typeof(GoogleAnalyticsViewComponent)
            );
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureOpenIdDict(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var linkedInOptions = configuration.GetSection(SettingsConsts.LinkedInOptionsSectionName).Get<ProviderOptions>();
        var indeedOptions = configuration.GetSection(SettingsConsts.IndeedOptionsSectionName).Get<ProviderOptions>();

        context.Services
            .AddOpenIddict()
            .AddServer(options =>
            {
                // TODO For production create self-signed certificates and store them in the X.509 certificates store  
                options.AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey();
                //if (context.Services.GetHostingEnvironment().IsDevelopment())
                //{
                //    options.AddEphemeralEncryptionKey()
                //       .AddEphemeralSigningKey();
                //}
                //else
                //{
                //    throw new NotImplementedException("You've got to resolve the X.509 certs store matter!");
                //}
            });

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

            .AddOAuth<OAuthOptions, BuildMyTalentOAuthLinkedInHandler>(SettingsConsts.LinkedInProviderName, SettingsConsts.LinkedInProviderName, configuration =>
            {

                configuration.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                configuration.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                configuration.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                configuration.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                configuration.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                configuration.ClientId = linkedInOptions.ClientId;
                configuration.ClientSecret = linkedInOptions.ClientSecret;
                configuration.ReturnUrlParameter = linkedInOptions.ReturnUrl;
                configuration.ClaimsIssuer = linkedInOptions.ClaimsIssuer;
                configuration.CallbackPath = linkedInOptions.CallbackPath;
                configuration.Scope.Add("openid");
                configuration.Scope.Add("profile");
                configuration.Scope.Add("email");
                configuration.AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization";
                configuration.TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";
                configuration.UserInformationEndpoint = "https://api.linkedin.com/v2/userinfo";
                configuration.SaveTokens = true;

            })
            .AddOAuth<OAuthOptions, BuildMyTalentOAuthIndeedHandler>(SettingsConsts.IndeedProviderName, SettingsConsts.IndeedProviderName, configuration =>
            {
                configuration.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                configuration.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                configuration.ClientId = indeedOptions.ClientId;
                configuration.ClientSecret = indeedOptions.ClientSecret;
                configuration.Scope.Add("email+offline_access");
                configuration.CallbackPath = indeedOptions.CallbackPath;
                configuration.AuthorizationEndpoint = "https://secure.indeed.com/oauth/v2/authorize";
                configuration.TokenEndpoint = "https://apis.indeed.com/oauth/v2/tokens";
                configuration.UserInformationEndpoint = "https://secure.indeed.com/v2/api/userinfo";
                configuration.SaveTokens = true;
            });
    }
    private void ConfigureUrls(Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureIdentityOptions(ServiceConfigurationContext context)
    {
        context.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            //options.SignIn.RequireConfirmedEmail = true;
            //options.SignIn.RequireConfirmedPhoneNumber = false;
        });
    }

    private void ConfigureHooks(Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        Configure<AbpLayoutHookOptions>(options =>
        {
            options.Add(
                LayoutHooks.PageContent.Last,
                typeof(SpacerViewComponent));
            options.Add(
                LayoutHooks.Body.Last,
                typeof(FooterViewComponent)
            );
        });

    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Get(BasicThemeBundles.Styles.Global)
                .AddFiles("/libs/bootstrap-icons/font/bootstrap-icons.css");
            options.StyleBundles.Configure(
                BasicThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
            options.ScriptBundles.Configure(
                "jQuery",
                bundle =>
                {
                    bundle.AddFiles("/libs/jquery/jquery.js");
                }
            );

        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<JobPortalWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<JobPortalDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Collabed.JobPortal.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<JobPortalDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Collabed.JobPortal.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<JobPortalApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Collabed.JobPortal.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<JobPortalApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Collabed.JobPortal.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<JobPortalWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic", "is"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Română"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            options.Languages.Add(new LanguageInfo("el", "el", "Ελληνικά"));
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new JobPortalMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(JobPortalApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "JobPortal API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        // temp
        //if (env.IsDevelopment())
        //{
        app.UseDeveloperExceptionPage();
        //}

        app.UseAbpRequestLocalization();
        //temp 
        //if (!env.IsDevelopment())
        //{
        app.UseErrorPage();
        //}

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "JobPortal API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
