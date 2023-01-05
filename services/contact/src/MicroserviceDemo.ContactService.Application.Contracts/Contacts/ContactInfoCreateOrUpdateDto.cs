namespace MicroserviceDemo.ContactService.Contacts;

public class ContactInfoCreateOrUpdateDto
{
    public ContactInfoType Type { get; set; }

    public string Value { get; set; }
}