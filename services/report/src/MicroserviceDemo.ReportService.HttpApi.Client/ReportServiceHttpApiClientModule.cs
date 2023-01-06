using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(ReportServiceApplicationContractsModule),
    typeof(AbpHttpClientModule)
)]
public class ReportServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(ReportServiceApplicationContractsModule).Assembly,
            ReportServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(
            options => { options.FileSets.AddEmbedded<ReportServiceHttpApiClientModule>(); }
        );
    }
}