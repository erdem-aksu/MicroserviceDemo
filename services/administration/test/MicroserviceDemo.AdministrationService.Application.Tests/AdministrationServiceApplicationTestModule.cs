using Volo.Abp.Modularity;

namespace MicroserviceDemo.AdministrationService
{
    [DependsOn(
        typeof(AdministrationServiceApplicationModule),
        typeof(AdministrationServiceDomainTestModule)
    )]
    public class AdministrationServiceApplicationTestModule : AbpModule
    {
    }
}