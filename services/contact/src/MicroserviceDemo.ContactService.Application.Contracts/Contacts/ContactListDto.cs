using System;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactListDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string SurName { get; set; }

    public string Company { get; set; }
}