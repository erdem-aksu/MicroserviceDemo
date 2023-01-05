using Volo.Abp.Reflection;

namespace MicroserviceDemo.ReportService.Permissions;

public class ReportServicePermissions
{
    public const string GroupName = "ReportService";

    public static class Reports
    {
        public const string Default = GroupName + ".Reports";

        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ReportServicePermissions));
    }
}