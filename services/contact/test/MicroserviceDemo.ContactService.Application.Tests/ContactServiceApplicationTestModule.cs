using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(ContactServiceApplicationModule),
    typeof(ContactServiceDomainTestModule)
)]
public class ContactServiceApplicationTestModule : AbpModule
{
}