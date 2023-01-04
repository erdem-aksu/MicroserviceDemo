using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace MicroserviceDemo.ContactService.Contacts;

public abstract class ContactCreateOrUpdateDtoBase : ExtensibleObject
{
    public string Name { get; set; }

    public string SurName { get; set; }

    public string Company { get; set; }

    public List<ContactInfoCreateOrUpdateDto> Info { get; set; }

    public ContactCreateOrUpdateDtoBase() : base(false)
    {
        Info = new List<ContactInfoCreateOrUpdateDto>();
    }
}