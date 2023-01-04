using Microsoft.AspNetCore.Components;
using Volo.Abp.UI.Navigation;

namespace MicroserviceDemo.Web.Shared
{
    public partial class NavMenuItem
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public ApplicationMenuItem MenuItem { get; set; }
    }
}