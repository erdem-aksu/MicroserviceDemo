namespace MicroserviceDemo.Web.Menus;

public class MicroserviceDemoWebMenus
{
    private const string Prefix = "Web";

    public const string Home = Prefix + ".Home";
    public const string Contacts = Prefix + ".Contacts";
    public const string Reports = Prefix + ".Reports";

    public static class Identity
    {
        public const string GroupName = Prefix + ".Identity";

        public const string Roles = GroupName + ".Roles";
        public const string Users = GroupName + ".Users";
    }
}