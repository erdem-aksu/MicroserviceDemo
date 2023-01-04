using Volo.Abp.Domain.Entities;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactUpdateDto : ContactCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}