using System;
using MicroserviceDemo.ReportService.EntityFrameworkCore;
using MicroserviceDemo.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace MicroserviceDemo.ReportService.DbMigrations;

public class ReportServiceDatabaseMigrationChecker : PendingEfCoreMigrationsChecker<ReportServiceDbContext>
{
    public ReportServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            ReportServiceDbProperties.ConnectionStringName
        )
    {
    }
}