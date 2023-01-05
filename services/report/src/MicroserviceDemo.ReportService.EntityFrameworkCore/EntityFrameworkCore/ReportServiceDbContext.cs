using MicroserviceDemo.ReportService.Reports;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MicroserviceDemo.ReportService.EntityFrameworkCore;

[ConnectionStringName(ReportServiceDbProperties.ConnectionStringName)]
public class ReportServiceDbContext : AbpDbContext<ReportServiceDbContext>, IReportServiceDbContext
{
    public DbSet<Report> Reports { get; set; }

    public ReportServiceDbContext(DbContextOptions<ReportServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureReportService();
    }
}