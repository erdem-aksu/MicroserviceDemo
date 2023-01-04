using Volo.Abp.Modularity;

namespace MicroserviceDemo.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceApplicationModule),
        typeof(IdentityServiceDomainTestModule)
    )]
    public class IdentityServiceApplicationTestModule : AbpModule
    {
    }
}