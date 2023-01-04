using System;
using System.Threading.Tasks;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.Web.Theme
{
    [Dependency(ReplaceServices = true)]
    public class MudBlazorUiNotificationService : IUiNotificationService, IScopedDependency
    {
        public ISnackbar Snackbar { get; }

        public MudBlazorUiNotificationService(ISnackbar snackbar)
        {
            Snackbar = snackbar;
        }

        public void Add(string message, Severity severity = Severity.Normal, Action<SnackbarOptions> configure = null)
        {
            Snackbar.Add(message, severity, configure);
        }

        public Task Info(string message, string title = null, Action<UiNotificationOptions> options = null)
        {
            Snackbar.Add(message, Severity.Info);

            return Task.CompletedTask;
        }

        public Task Success(string message, string title = null, Action<UiNotificationOptions> options = null)
        {
            Snackbar.Add(message, Severity.Success);

            return Task.CompletedTask;
        }

        public Task Warn(string message, string title = null, Action<UiNotificationOptions> options = null)
        {
            Snackbar.Add(message, Severity.Warning);

            return Task.CompletedTask;
        }

        public Task Error(string message, string title = null, Action<UiNotificationOptions> options = null)
        {
            Snackbar.Add(message, Severity.Error);

            return Task.CompletedTask;
        }
    }
}