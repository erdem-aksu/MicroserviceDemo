using System;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactInfoDto : AuditedEntityDto<Guid>
{
    public Guid ContactId { get; set; }

    public ContactInfoType Type { get; set; }

    public string Value { get; set; }
}