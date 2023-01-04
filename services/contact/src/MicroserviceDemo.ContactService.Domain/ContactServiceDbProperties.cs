namespace MicroserviceDemo.ContactService;

public static class ContactServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ContactService";
}