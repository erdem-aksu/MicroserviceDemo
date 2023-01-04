using MicroserviceDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MicroserviceDemo.Web;

public abstract class WebComponentBase : AbpComponentBase
{
    protected WebComponentBase()
    {
        LocalizationResource = typeof(MicroserviceDemoResource);
    }
}