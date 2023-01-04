using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace MicroserviceDemo.Shared.Hosting.AspNetCore;

[DependsOn(
    typeof(MicroserviceDemoSharedHostingModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class MicroserviceDemoSharedHostingAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<ForwardedHeadersOptions>(
            options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.ForwardLimit = 2;
                foreach (var ip in configuration.GetSection("KnownProxies").Get<string[]>() ?? Array.Empty<string>())
                {
                    if (ip.Contains('/'))
                    {
                        var ipBlock = ip.Split('/');
                        options.KnownNetworks.Add(
                            new IPNetwork(
                                IPAddress.Parse(ipBlock[0]),
                                Convert.ToInt32(ipBlock[1])
                            )
                        );
                    }
                    else
                    {
                        options.KnownProxies.Add(IPAddress.Parse(ip));
                    }
                }
            }
        );
    }
}