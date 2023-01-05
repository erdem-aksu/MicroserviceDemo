using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace MicroserviceDemo.ContactService.EntityFrameworkCore;

[DependsOn(
    typeof(ContactServiceDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule)
)]
public class ContactServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ContactServiceDbContext>(
            options => { options.AddDefaultRepositories(includeAllEntities: true); }
        );

        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        Configure<AbpDbContextOptions>(
            options =>
            {
                options.Configure<ContactServiceDbContext>(
                    c => { c.UseNpgsql(b => { b.MigrationsHistoryTable("__ContactService_Migrations"); }); }
                );
            }
        );
    }
}