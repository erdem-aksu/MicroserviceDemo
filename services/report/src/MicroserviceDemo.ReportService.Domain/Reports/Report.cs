using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroserviceDemo.ReportService.Reports;

public class Report : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public ReportStatus Status { get; set; }

    private Report()
    {
    }

    public Report(Guid id) : base(id)
    {
        Status = ReportStatus.Pending;
    }

    public Report(Guid id, string name, ReportStatus status) : base(id)
    {
        Name = name;
        Status = status;
    }
}