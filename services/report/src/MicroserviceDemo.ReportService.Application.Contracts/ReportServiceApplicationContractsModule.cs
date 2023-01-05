using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(ReportServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
)]
public class ReportServiceApplicationContractsModule : AbpModule
{
}