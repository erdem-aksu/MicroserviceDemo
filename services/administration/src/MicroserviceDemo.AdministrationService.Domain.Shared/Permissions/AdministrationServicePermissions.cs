namespace MicroserviceDemo.AdministrationService.Permissions
{
    public static class AdministrationServicePermissions
    {
        public const string GroupName = "AdministrationService";

        public const string Dashboard = GroupName + ".Dashboard";
        public const string HangfireDashboard = GroupName + ".HangfireDashboard";

        public static class Identity
        {
            public const string Default = GroupName + ".Identity";

            public static class Roles
            {
                public const string Default = Identity.Default + ".Roles";

                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
                public const string ManagePermissions = Default + ".ManagePermissions";
            }

            public static class Users
            {
                public const string Default = Identity.Default + ".Users";

                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
                public const string ManagePermissions = Default + ".ManagePermissions";
                public const string Impersonation = Default + ".Impersonation";
            }

            public static class UserLookup
            {
                public const string Default = Identity.Default + ".UserLookup";
            }
        }

        public static class Settings
        {
            public const string Default = GroupName + ".SettingManagement";

            public const string Host = Default + ".Host";
        }
    }
}