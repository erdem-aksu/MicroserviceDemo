using MicroserviceDemo.IdentityService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceEntityFrameworkCoreTestModule)
    )]
    public class IdentityServiceDomainTestModule : AbpModule
    {
    }
}