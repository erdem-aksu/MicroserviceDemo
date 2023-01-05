using MicroserviceDemo.AdministrationService;
using MicroserviceDemo.ContactService;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(ReportServiceDomainModule),
    typeof(ReportServiceApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AdministrationServiceHttpApiClientModule),
    typeof(ContactServiceHttpApiClientModule)
)]
public class ReportServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ReportServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<ReportServiceApplicationModule>(validate: true); });
    }
}