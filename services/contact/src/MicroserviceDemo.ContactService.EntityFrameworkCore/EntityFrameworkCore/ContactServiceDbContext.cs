using MicroserviceDemo.ContactService.Contacts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MicroserviceDemo.ContactService.EntityFrameworkCore;

[ConnectionStringName(ContactServiceDbProperties.ConnectionStringName)]
public class ContactServiceDbContext : AbpDbContext<ContactServiceDbContext>, IContactServiceDbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public DbSet<ContactInfo> ContactInfo { get; set; }

    public ContactServiceDbContext(DbContextOptions<ContactServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureContactService();
    }
}