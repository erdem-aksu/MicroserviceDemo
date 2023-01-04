using MicroserviceDemo.AdministrationService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.AdministrationService
{
    [DependsOn(
        typeof(AdministrationServiceEntityFrameworkCoreTestModule)
    )]
    public class AdministrationServiceDomainTestModule : AbpModule
    {
    }
}