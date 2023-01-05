using MicroserviceDemo.ReportService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ReportService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(ReportServiceEntityFrameworkCoreTestModule)
)]
public class ReportServiceDomainTestModule : AbpModule
{
}