using Localization.Resources.AbpUi;
using MicroserviceDemo.ContactService.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(ContactServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class ContactServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(
            mvcBuilder => { mvcBuilder.AddApplicationPartIfNotExists(typeof(ContactServiceHttpApiModule).Assembly); }
        );
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(
            options =>
            {
                options.Resources
                    .Get<ContactServiceResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            }
        );
    }
}