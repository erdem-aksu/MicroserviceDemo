using Volo.Abp.Reflection;

namespace MicroserviceDemo.ContactService.Permissions;

public class ContactServicePermissions
{
    public const string GroupName = "ContactService";

    public static class Contacts
    {
        public const string Default = GroupName + ".Contacts";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ContactServicePermissions));
    }
}