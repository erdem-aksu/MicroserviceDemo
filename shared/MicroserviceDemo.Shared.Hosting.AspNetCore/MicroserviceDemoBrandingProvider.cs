using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MicroserviceDemo.Shared.Hosting.AspNetCore
{
    [Dependency(ReplaceServices = true)]
    public class MicroserviceDemoBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "MicroserviceDemo";
    }
}