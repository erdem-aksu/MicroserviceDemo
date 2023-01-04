using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactDto : ExtensibleCreationAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public string SurName { get; set; }

    public string Company { get; set; }

    public List<ContactInfoDto> Info { get; set; }

    public string ConcurrencyStamp { get; set; }
}