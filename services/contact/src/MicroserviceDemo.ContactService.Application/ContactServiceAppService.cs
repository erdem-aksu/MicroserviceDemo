using MicroserviceDemo.ContactService.Localization;
using Volo.Abp.Application.Services;

namespace MicroserviceDemo.ContactService;

public abstract class ContactServiceAppService : ApplicationService
{
    protected ContactServiceAppService()
    {
        LocalizationResource = typeof(ContactServiceResource);
        ObjectMapperContext = typeof(ContactServiceApplicationModule);
    }
}