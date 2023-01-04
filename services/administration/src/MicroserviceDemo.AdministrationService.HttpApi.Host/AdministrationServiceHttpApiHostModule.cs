using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroserviceDemo.AdministrationService.DbMigrations;
using MicroserviceDemo.AdministrationService.EntityFrameworkCore;
using MicroserviceDemo.Shared.Hosting.AspNetCore;
using MicroserviceDemo.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.AdministrationService;

[DependsOn(
    typeof(AdministrationServiceHttpApiModule),
    typeof(AdministrationServiceApplicationModule),
    typeof(AdministrationServiceEntityFrameworkCoreModule),
    typeof(MicroserviceDemoSharedHostingMicroservicesModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpIdentityHttpApiClientModule)
)]
public class AdministrationServiceHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        JwtBearerConfigurationHelper.Configure(context, "AdministrationService");

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    { "AdministrationService", "Administration Service API" }
                },
            apiTitle: "Administration Service API"
        );

        context.Services.AddCors(
            options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                configuration["App:CorsOrigins"]
                                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(o => o.Trim().RemovePostFix("/"))
                                    .ToArray()
                            )
                            .WithAbpExposedHeaders()
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    }
                );
            }
        );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseCorrelationId();
        app.UseCors();
        app.UseForwardedHeaders();
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Administration Service API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                // options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            }
        );
        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<AdministrationServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}