using MicroserviceDemo.ContactService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroserviceDemo.ContactService;

public abstract class ContactServiceController : AbpControllerBase
{
    protected ContactServiceController()
    {
        LocalizationResource = typeof(ContactServiceResource);
    }
}