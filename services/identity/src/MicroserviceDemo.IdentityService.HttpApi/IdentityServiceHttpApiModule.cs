using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceApplicationContractsModule),
        typeof(AbpIdentityHttpApiModule)
    )]
    public class IdentityServiceHttpApiModule : AbpModule
    {
    }
}