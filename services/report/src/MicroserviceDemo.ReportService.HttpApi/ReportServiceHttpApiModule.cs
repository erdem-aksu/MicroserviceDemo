using Localization.Resources.AbpUi;
using MicroserviceDemo.ReportService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(ReportServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class ReportServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => { mvcBuilder.AddApplicationPartIfNotExists(typeof(ReportServiceHttpApiModule).Assembly); });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(
            options =>
            {
                options.Resources
                    .Get<ReportServiceResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            }
        );
    }
}