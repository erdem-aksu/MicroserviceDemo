using System;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ReportService.Reports;

public class ReportListDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public ReportStatus Status { get; set; }
}