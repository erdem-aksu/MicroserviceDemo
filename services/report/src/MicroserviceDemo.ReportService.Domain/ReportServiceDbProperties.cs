namespace MicroserviceDemo.ReportService;

public static class ReportServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ReportService";
}