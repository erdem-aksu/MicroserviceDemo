using System;
using System.IO;
using MicroserviceDemo.AdministrationService;
using MicroserviceDemo.ContactService;
using MicroserviceDemo.IdentityService;
using MicroserviceDemo.Localization;
using MicroserviceDemo.ReportService;
using MicroserviceDemo.Shared.Hosting.AspNetCore;
using MicroserviceDemo.Web.Bundling;
using MicroserviceDemo.Web.Menus;
using MicroserviceDemo.Web.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MudBlazor.Services;
using Polly;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Components.Server;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.FontAwesome;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.FluentValidation;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace MicroserviceDemo.Web;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMvcClientModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpFluentValidationModule),
    typeof(AbpAspNetCoreComponentsServerModule),
    typeof(AbpAspNetCoreMvcUiPackagesModule),
    typeof(AbpAspNetCoreMvcUiBundlingModule),
    typeof(MicroserviceDemoSharedHostingAspNetCoreModule),
    typeof(MicroserviceDemoSharedLocalizationModule),
    typeof(AdministrationServiceHttpApiClientModule),
    typeof(IdentityServiceHttpApiClientModule),
    typeof(ContactServiceHttpApiClientModule),
    typeof(ReportServiceHttpApiClientModule)
)]
public class MicroserviceDemoWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(
            options =>
            {
                options.AddAssemblyResource(
                    typeof(MicroserviceDemoResource),
                    typeof(MicroserviceDemoWebModule).Assembly
                );
            }
        );

        PreConfigure<AbpHttpClientBuilderOptions>(
            options =>
            {
                options.ProxyClientBuildActions.Add(
                    (remoteServiceName, clientBuilder) =>
                    {
                        clientBuilder.AddTransientHttpErrorPolicy(
                            policyBuilder =>
                                policyBuilder.WaitAndRetryAsync(
                                    3,
                                    i => TimeSpan.FromSeconds(Math.Pow(2, i))
                                )
                        );
                    }
                );
            }
        );
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IdentityModelEventSource.ShowPII = true;

        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureUrls(configuration);
        ConfigureCache();
        ConfigureBundles(context, hostingEnvironment);
        ConfigureMultiTenancy();
        ConfigureAuthentication(context, configuration);
        ConfigureAutoMapper(context);
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureLocalizationServices();
        ConfigureHttpClient(context);
        ConfigureBlazorComponents(context, configuration);
        ConfigureRouter(context);
        ConfigureMenu(configuration);
        ConfigureDataProtection(context, configuration);
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(
            options => { options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"]; }
        );

        Configure<AbpRemoteServiceOptions>(
            options => { options.RemoteServices.Default = new RemoteServiceConfiguration(configuration["RemoteServices:Default:BaseUrl"]); }
        );
    }

    private void ConfigureCache()
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "MicroserviceDemo:"; });
    }

    private void ConfigureBundles(ServiceConfigurationContext context, IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpBundlingOptions>(
            options =>
            {
                options.Mode = hostingEnvironment.IsDevelopment()
                    ? BundlingMode.None
                    : BundlingMode.BundleAndMinify;

                //BLAZOR UI
                options.StyleBundles.Configure(
                    MicroserviceDemoBlazorBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.Contributors.Remove<BootstrapStyleContributor>();
                        bundle.Contributors.Remove<FontAwesomeStyleContributor>();

                        bundle.Contributors.Add<MicroserviceDemoWebStyleContributor>();

                        bundle.AddFiles("/_content/MudBlazor/MudBlazor.min.css");
                        bundle.AddFiles("/_content/MudBlazor.ThemeManager/MudBlazorThemeManager.css");

                        //You can remove the following line if you don't use Blazor CSS isolation for components
                        bundle.AddFiles("/MicroserviceDemo.Web.styles.css");
                    }
                );
                options.ScriptBundles.Configure(
                    MicroserviceDemoBlazorBundles.Scripts.Global,
                    bundle =>
                    {
                        bundle.Contributors.Remove<BootstrapScriptContributor>();

                        bundle.AddFiles("/_framework/blazor.server.js");
                        // bundle.AddFiles("/_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");

                        bundle.Contributors.Add<MicroserviceDemoWebScriptContributor>();

                        bundle.AddFiles("/_content/MudBlazor/MudBlazor.min.js");
                    }
                );
            }
        );
    }

    private void ConfigureMultiTenancy()
    {
        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = true; });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                }
            )
            .AddCookie("Cookies")
            .AddAbpOpenIdConnect(
                "oidc",
                options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                    options.ClientId = configuration["AuthServer:ClientId"];
                    options.ClientSecret = configuration["AuthServer:ClientSecret"];

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("email");
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("phone");
                    options.Scope.Add("role");
                    options.Scope.Add("AdministrationService");
                    options.Scope.Add("IdentityService");
                    options.Scope.Add("ContactService");
                    options.Scope.Add("ReportService");
                }
            );
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(
            options =>
            {
                options.FileSets.AddEmbedded<MicroserviceDemoWebModule>();
                if (hostingEnvironment.IsDevelopment())
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<MicroserviceDemoWebModule>(hostingEnvironment.ContentRootPath);
                    options.FileSets.ReplaceEmbeddedByPhysical<MicroserviceDemoSharedLocalizationModule>(
                        Path.Combine(
                            hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}shared{Path.DirectorySeparatorChar}MicroserviceDemo.Shared.Localization"
                        )
                    );
                }
            }
        );
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(
            options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English", "gb"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "tr"));
            }
        );
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
    }

    private void ConfigureBlazorComponents(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddMudServices();

        // context.Services
        // .AddBootstrapProviders()
        // .AddFontAwesomeIcons();
    }

    private void ConfigureMenu(IConfiguration configuration)
    {
        Configure<AbpNavigationOptions>(
            options => { options.MenuContributors.Add(new MicroserviceDemoWebMenuContributor(configuration)); }
        );
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(
            options => { options.AppAssembly = typeof(MicroserviceDemoWebModule).Assembly; }
        );
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<MicroserviceDemoWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<MicroserviceDemoWebModule>(); });
    }

    private void ConfigureDataProtection(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        context.Services.AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "MicroserviceDemo-Protection-Keys")
            .SetApplicationName("Web");
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseStatusCodePagesWithRedirects("~/Error?httpStatusCode={0}")
                .UseExceptionHandler("/Error");
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseForwardedHeaders();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}