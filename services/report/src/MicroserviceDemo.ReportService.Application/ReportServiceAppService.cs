using MicroserviceDemo.ReportService.Localization;
using Volo.Abp.Application.Services;

namespace MicroserviceDemo.ReportService;

public abstract class ReportServiceAppService : ApplicationService
{
    protected ReportServiceAppService()
    {
        LocalizationResource = typeof(ReportServiceResource);
        ObjectMapperContext = typeof(ReportServiceApplicationModule);
    }
}