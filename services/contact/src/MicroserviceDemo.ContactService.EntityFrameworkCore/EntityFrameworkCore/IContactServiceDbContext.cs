using MicroserviceDemo.ContactService.Contacts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MicroserviceDemo.ContactService.EntityFrameworkCore;

[ConnectionStringName(ContactServiceDbProperties.ConnectionStringName)]
public interface IContactServiceDbContext : IEfCoreDbContext
{
    DbSet<Contact> Contacts { get; }

    DbSet<ContactInfo> ContactInfo { get; }
}