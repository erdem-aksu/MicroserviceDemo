using MicroserviceDemo.ContactService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(ContactServiceEntityFrameworkCoreTestModule)
)]
public class ContactServiceDomainTestModule : AbpModule
{
}