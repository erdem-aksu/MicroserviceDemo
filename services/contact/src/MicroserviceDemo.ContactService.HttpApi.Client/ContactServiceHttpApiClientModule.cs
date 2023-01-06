using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(ContactServiceApplicationContractsModule),
    typeof(AbpHttpClientModule)
)]
public class ContactServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(ContactServiceApplicationContractsModule).Assembly,
            ContactServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(
            options => { options.FileSets.AddEmbedded<ContactServiceHttpApiClientModule>(); }
        );
    }
}