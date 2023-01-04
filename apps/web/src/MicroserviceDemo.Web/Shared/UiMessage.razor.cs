using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;

namespace MicroserviceDemo.Web.Shared
{
    public partial class UiMessage : ComponentBase, IDisposable
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public string ContentText { get; set; }

        [Parameter]
        public Color Color { get; set; }

        [Parameter]
        public bool IsConfirm { get; set; }

        [Parameter]
        public UiMessageOptions UiMessageOptions { get; set; }

        private void Submit() => MudDialog.Close(DialogResult.Ok(true));
        private void Cancel() => MudDialog.Cancel();
        private void Close() => MudDialog.Close();

        public void Dispose()
        {
            MudDialog?.Close();
        }
    }
}