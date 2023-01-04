using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(ContactServiceDomainModule),
    typeof(ContactServiceApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
)]
public class ContactServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ContactServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(
            options => { options.AddMaps<ContactServiceApplicationModule>(validate: true); }
        );
    }
}