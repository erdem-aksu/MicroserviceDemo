using MicroserviceDemo.ReportService.Reports;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MicroserviceDemo.ReportService.EntityFrameworkCore;

[ConnectionStringName(ReportServiceDbProperties.ConnectionStringName)]
public interface IReportServiceDbContext : IEfCoreDbContext
{
    DbSet<Report> Reports { get; }
}