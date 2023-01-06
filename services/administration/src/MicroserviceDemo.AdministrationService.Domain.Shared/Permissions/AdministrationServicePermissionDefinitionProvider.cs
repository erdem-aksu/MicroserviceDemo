using MicroserviceDemo.AdministrationService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace MicroserviceDemo.AdministrationService.Permissions
{
    public class AdministrationServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public const string ClientPermissionValueProviderName = "C";

        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(AdministrationServicePermissions.GroupName);

            group.AddPermission(AdministrationServicePermissions.Dashboard, L("Permission:Dashboard"));
            group.AddPermission(AdministrationServicePermissions.HangfireDashboard, L("Permission:HangfireDashboard"), MultiTenancySides.Host);

            var rolesPermission = group.AddPermission(AdministrationServicePermissions.Identity.Roles.Default, L("Permission:RoleManagement"));
            rolesPermission.AddChild(AdministrationServicePermissions.Identity.Roles.Create, L("Permission:Create"));
            rolesPermission.AddChild(AdministrationServicePermissions.Identity.Roles.Update, L("Permission:Edit"));
            rolesPermission.AddChild(AdministrationServicePermissions.Identity.Roles.Delete, L("Permission:Delete"));
            rolesPermission.AddChild(AdministrationServicePermissions.Identity.Roles.ManagePermissions, L("Permission:ChangePermissions"));

            var usersPermission = group.AddPermission(AdministrationServicePermissions.Identity.Users.Default, L("Permission:UserManagement"));
            usersPermission.AddChild(AdministrationServicePermissions.Identity.Users.Create, L("Permission:Create"));
            usersPermission.AddChild(AdministrationServicePermissions.Identity.Users.Update, L("Permission:Edit"));
            usersPermission.AddChild(AdministrationServicePermissions.Identity.Users.Delete, L("Permission:Delete"));
            usersPermission.AddChild(AdministrationServicePermissions.Identity.Users.ManagePermissions, L("Permission:ChangePermissions"));
            usersPermission.AddChild(AdministrationServicePermissions.Identity.Users.Impersonation, L("Permission:Impersonation"));

            group.AddPermission(AdministrationServicePermissions.Identity.UserLookup.Default, L("Permission:UserLookup"))
                .WithProviders(ClientPermissionValueProviderName);

            var settingsPermission = group.AddPermission(AdministrationServicePermissions.Settings.Default, L("Permission:SettingManagement"));
            settingsPermission.AddChild(AdministrationServicePermissions.Settings.Host, L("Permission:SettingManagement.Host"), MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AdministrationServiceResource>(name);
        }
    }
}