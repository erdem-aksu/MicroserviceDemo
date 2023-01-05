using System;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ReportService.Reports;

public class ReportDto : ExtensibleCreationAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public ReportStatus Status { get; set; }
}