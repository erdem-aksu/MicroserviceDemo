using Volo.Abp.Modularity;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(ReportServiceApplicationModule),
    typeof(ReportServiceDomainTestModule)
)]
public class ReportServiceApplicationTestModule : AbpModule
{
}