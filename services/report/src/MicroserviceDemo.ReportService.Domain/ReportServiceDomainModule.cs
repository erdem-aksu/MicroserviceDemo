using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ReportServiceDomainSharedModule)
)]
public class ReportServiceDomainModule : AbpModule
{
}