using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactInfo : FullAuditedAggregateRoot<Guid>
{
    public Guid ContactId { get; set; }

    public ContactInfoType Type { get; set; }

    public string Value { get; set; }

    public Contact Contact { get; set; }

    private ContactInfo()
    {
    }

    public ContactInfo(Guid id, Guid contactId, ContactInfoType type, string value) : base(id)
    {
        ContactId = contactId;
        Type = type;
        Value = value;
    }

    public ContactInfo(Guid id, ContactInfoType type, string value) : base(id)
    {
        Type = type;
        Value = value;
    }
}