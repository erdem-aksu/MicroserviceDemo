using System;
using MicroserviceDemo.ContactService.EntityFrameworkCore;
using MicroserviceDemo.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace MicroserviceDemo.ContactService.DbMigrations;

public class ContactServiceDatabaseMigrationChecker : PendingEfCoreMigrationsChecker<ContactServiceDbContext>
{
    public ContactServiceDatabaseMigrationChecker(
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
            ContactServiceDbProperties.ConnectionStringName
        )
    {
    }
}