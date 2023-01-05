using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ReportService.Reports;

public class GetReportsInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}