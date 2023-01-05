using Volo.Abp.ObjectExtending;

namespace MicroserviceDemo.ReportService.Reports;

public class ReportCreateDto : ExtensibleObject
{
    public string Location { get; set; }
}