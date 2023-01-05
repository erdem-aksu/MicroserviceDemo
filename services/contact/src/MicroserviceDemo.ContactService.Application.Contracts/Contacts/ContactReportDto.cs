namespace MicroserviceDemo.ContactService.Contacts;

public class ContactReportDto
{
    public string Location { get; set; }

    public long ContactCount { get; set; }

    public long ContactPhoneCount { get; set; }
}