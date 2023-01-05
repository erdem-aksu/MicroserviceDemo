using MicroserviceDemo.AdministrationService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroserviceDemo.AdministrationService;

public abstract class AdministrationServiceController : AbpControllerBase
{
    protected AdministrationServiceController()
    {
        LocalizationResource = typeof(AdministrationServiceResource);
    }
}