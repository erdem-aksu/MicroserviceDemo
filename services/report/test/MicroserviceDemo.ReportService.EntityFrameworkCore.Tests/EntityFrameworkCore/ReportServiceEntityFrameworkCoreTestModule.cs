using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ReportService.EntityFrameworkCore;

[DependsOn(
    typeof(ReportServiceTestBaseModule),
    typeof(ReportServiceEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class ReportServiceEntityFrameworkCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<AbpDbContextOptions>(options => { options.Configure(abpDbContextConfigurationContext => { abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection); }); });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        new ReportServiceDbContext(
                new DbContextOptionsBuilder<ReportServiceDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>()
            .CreateTables();

        return connection;
    }
}