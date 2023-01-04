using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ContactService.Contacts;

public class GetContactsInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public string Location { get; set; }
}