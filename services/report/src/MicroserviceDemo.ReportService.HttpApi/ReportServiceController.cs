using MicroserviceDemo.ReportService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroserviceDemo.ReportService;

public abstract class ReportServiceController : AbpControllerBase
{
    protected ReportServiceController()
    {
        LocalizationResource = typeof(ReportServiceResource);
    }
}