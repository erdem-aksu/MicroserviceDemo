namespace MicroserviceDemo.Web.Menus;

public class MicroserviceDemoWebMenus
{
    private const string Prefix = "Web";

    public const string Home = Prefix + ".Home";

    public static class Identity
    {
        public const string GroupName = Prefix + ".Identity";

        public const string Roles = GroupName + ".Roles";
        public const string Users = GroupName + ".Users";
    }
}