using System;
using System.Threading.Tasks;
using MicroserviceDemo.AdministrationService.Permissions;
using MicroserviceDemo.ContactService.Permissions;
using MicroserviceDemo.Localization;
using MicroserviceDemo.ReportService.Permissions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace MicroserviceDemo.Web.Menus;

public class MicroserviceDemoWebMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public MicroserviceDemoWebMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        administration.Icon = Icons.Material.Sharp.SettingsApplications;

        var l = context.GetLocalizer<MicroserviceDemoResource>();

        context.Menu.Items.Add(
            new ApplicationMenuItem(
                MicroserviceDemoWebMenus.Home,
                l["Menu:Home"],
                "/",
                Icons.Material.Sharp.Home,
                0,
                requiredPermissionName: AdministrationServicePermissions.Dashboard
            )
        );

        context.Menu.Items.Add(
            new ApplicationMenuItem(
                MicroserviceDemoWebMenus.Contacts,
                l["Menu:Contacts"],
                "/contacts",
                Icons.Material.Sharp.Group,
                1,
                requiredPermissionName: ContactServicePermissions.Contacts.Default
            )
        );

        context.Menu.Items.Add(
            new ApplicationMenuItem(
                MicroserviceDemoWebMenus.Reports,
                l["Menu:Reports"],
                "/reports",
                Icons.Material.Sharp.Report,
                2,
                requiredPermissionName: ReportServicePermissions.Reports.Default
            )
        );

        administration.AddItem(
            new ApplicationMenuItem(
                    MicroserviceDemoWebMenus.Identity.GroupName,
                    l["Menu:IdentityManagement"],
                    icon: Icons.Material.Sharp.AssignmentInd,
                    order: 1
                ).AddItem(
                    new ApplicationMenuItem(
                        MicroserviceDemoWebMenus.Identity.Roles,
                        l["Menu:IdentityManagement.Roles"],
                        "~/identity/roles",
                        requiredPermissionName: AdministrationServicePermissions.Identity.Roles.Default
                    )
                )
                .AddItem(
                    new ApplicationMenuItem(
                        MicroserviceDemoWebMenus.Identity.Users,
                        l["Menu:IdentityManagement.Users"],
                        "~/identity/users",
                        requiredPermissionName: AdministrationServicePermissions.Identity.Users.Default
                    )
                )
        );

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<MicroserviceDemoResource>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        var identityServerUrl = _configuration["AuthServer:Authority"] ?? "";

        if (currentUser.IsAuthenticated)
        {
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "Account.Manage",
                    l["ManageYourProfile"],
                    $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
                    icon: "fa fa-cog",
                    order: 1000,
                    null,
                    "_blank"
                )
            );

            if (currentUser.FindImpersonatorUserId().HasValue)
            {
                context.Menu.AddItem(new ApplicationMenuItem("Account.BackToImpersonator", l["BackToImpersonator"], url: "~/Account/BackToImpersonator", icon: "fa fa-power-off", order: int.MaxValue - 1000));
            }

            context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", l["Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000));
        }

        return Task.CompletedTask;
    }
}