using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(ContactServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
)]
public class ContactServiceApplicationContractsModule : AbpModule
{
}