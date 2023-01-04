namespace MicroserviceDemo.ContactService.Contacts;

public abstract class ContactInfoCreateOrUpdateDto
{
    public ContactInfoType Type { get; set; }

    public string Value { get; set; }
}