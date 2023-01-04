using MicroserviceDemo.Shared.Hosting.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.Shared.Hosting.Gateways
{
    [DependsOn(
        typeof(MicroserviceDemoSharedHostingAspNetCoreModule)
    )]
    public class MicroserviceDemoSharedHostingGatewaysModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }
    }
}