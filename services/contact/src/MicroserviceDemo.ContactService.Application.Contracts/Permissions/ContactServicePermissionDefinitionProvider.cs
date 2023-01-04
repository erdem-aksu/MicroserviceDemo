using MicroserviceDemo.ContactService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MicroserviceDemo.ContactService.Permissions;

public class ContactServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(ContactServicePermissions.GroupName, L("Permission:ContactService"));

        var contactsPermission = group.AddPermission(ContactServicePermissions.Contacts.Default, L("Permission:Contacts"));
        contactsPermission.AddChild(ContactServicePermissions.Contacts.Create, L("Permission:Create"));
        contactsPermission.AddChild(ContactServicePermissions.Contacts.Update, L("Permission:Edit"));
        contactsPermission.AddChild(ContactServicePermissions.Contacts.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ContactServiceResource>(name);
    }
}