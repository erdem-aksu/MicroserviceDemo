using System.Collections.Generic;
using System.Threading.Tasks;
using MudBlazor;

namespace MicroserviceDemo.Web
{
    public abstract class WebPageBase : WebComponentBase
    {
        protected List<BreadcrumbItem> BreadcrumbItems { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            await SetBreadcrumbItemsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}