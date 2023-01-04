using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroserviceDemo.ContactService.Contacts;

public class Contact : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public string SurName { get; set; }

    public string Company { get; set; }

    public ICollection<ContactInfo> Info { get; }

    private Contact()
    {
        Info = new Collection<ContactInfo>();
    }

    public Contact(Guid id, string name, string surName, string company) : base(id)
    {
        Name = name;
        SurName = surName;
        Company = company;

        Info = new Collection<ContactInfo>();
    }
}