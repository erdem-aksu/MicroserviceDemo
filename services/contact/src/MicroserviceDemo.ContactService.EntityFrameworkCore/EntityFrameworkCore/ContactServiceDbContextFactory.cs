using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MicroserviceDemo.ContactService.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands)
     * */
    public class ContactServiceDbContextFactory : IDesignTimeDbContextFactory<ContactServiceDbContext>
    {
        public ContactServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ContactServiceDbContext>()
                .UseNpgsql(GetConnectionStringFromConfiguration(), b => { b.MigrationsHistoryTable("__ContactService_Migrations"); });

            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return new ContactServiceDbContext(builder.Options);
        }

        private static string GetConnectionStringFromConfiguration()
        {
            return BuildConfiguration()
                .GetConnectionString(ContactServiceDbProperties.ConnectionStringName);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        $"..{Path.DirectorySeparatorChar}MicroserviceDemo.ContactService.HttpApi.Host"
                    )
                )
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}