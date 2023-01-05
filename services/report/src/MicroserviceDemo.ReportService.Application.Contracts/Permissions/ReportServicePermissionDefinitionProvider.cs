using MicroserviceDemo.ReportService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MicroserviceDemo.ReportService.Permissions;

public class ReportServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(ReportServicePermissions.GroupName, L("Permission:ReportService"));

        var reportsPermission = group.AddPermission(ReportServicePermissions.Reports.Default, L("Permission:Reports"));
        reportsPermission.AddChild(ReportServicePermissions.Reports.Create, L("Permission:Create"));
        reportsPermission.AddChild(ReportServicePermissions.Reports.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ReportServiceResource>(name);
    }
}