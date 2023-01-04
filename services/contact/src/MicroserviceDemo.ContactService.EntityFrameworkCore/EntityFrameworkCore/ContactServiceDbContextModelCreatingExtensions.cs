using MicroserviceDemo.ContactService.Contacts;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MicroserviceDemo.ContactService.EntityFrameworkCore;

public static class ContactServiceDbContextModelCreatingExtensions
{
    public static void ConfigureContactService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Contact>(
            b =>
            {
                b.ToTable(ContactServiceDbProperties.DbTablePrefix + "Contacts", ContactServiceDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.HasMany(p => p.Info).WithOne(c => c.Contact).HasForeignKey(c => c.ContactId).IsRequired();
            }
        );

        builder.Entity<ContactInfo>(
            b =>
            {
                b.ToTable(ContactServiceDbProperties.DbTablePrefix + "ContactInfo", ContactServiceDbProperties.DbSchema);

                b.ConfigureByConvention();

                // b.HasIndex(c => new {c.ContactId, c.Type}).IsUnique();
            }
        );
    }
}