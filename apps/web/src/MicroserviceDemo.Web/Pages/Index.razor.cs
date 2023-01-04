using System;
using System.Threading.Tasks;
using MicroserviceDemo.AdministrationService.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace MicroserviceDemo.Web.Pages
{
    public partial class Index
    {
        protected override async Task OnInitializedAsync()
        {
            var hasPermission = await AuthorizationService.IsGrantedAsync(AdministrationServicePermissions.Identity.Users.Create);
        }

    }
}