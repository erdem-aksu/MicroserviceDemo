using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ContactServiceDomainSharedModule)
)]
public class ContactServiceDomainModule : AbpModule
{
}