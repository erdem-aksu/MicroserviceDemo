using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ReportService.EntityFrameworkCore;

[DependsOn(
    typeof(ReportServiceDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule)
)]
public class ReportServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ReportServiceDbContext>(
            options => { options.AddDefaultRepositories(includeAllEntities: true); }
        );

        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        Configure<AbpDbContextOptions>(
            options =>
            {
                options.Configure<ReportServiceDbContext>(
                    c => { c.UseNpgsql(b => { b.MigrationsHistoryTable("__ReportService_Migrations"); }); }
                );
            }
        );
    }
}