using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using MicroserviceDemo.Web.Shared;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.Web.Theme
{
    [Dependency(ReplaceServices = true)]
    public class MudBlazorUiMessageService : IUiMessageService, IScopedDependency
    {
        private readonly IStringLocalizer<AbpUiResource> _localizer;

        public ILogger<MudBlazorUiMessageService> Logger { get; set; }

        public IDialogService DialogService { get; set; }

        public MudBlazorUiMessageService(
            IStringLocalizer<AbpUiResource> localizer,
            IDialogService dialogService)
        {
            _localizer = localizer;
            DialogService = dialogService;
            Logger = NullLogger<MudBlazorUiMessageService>.Instance;
        }

        public Task Info(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            var uiMessageOptions = CreateDefaultUiMessageOptions();
            uiMessageOptions.MessageIcon = Icons.Material.Filled.Info;
            options?.Invoke(uiMessageOptions);
            
            var dialogOptions = CreateDefaultOptions();

            var parameters = new DialogParameters
            {
                { "UiMessageOptions", uiMessageOptions },
                { "ContentText", message },
                { "Color", Color.Info }
            };

            DialogService.Show<UiMessage>(title, parameters, dialogOptions);

            return Task.CompletedTask;
        }

        public Task Success(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            var uiMessageOptions = CreateDefaultUiMessageOptions();
            uiMessageOptions.MessageIcon = Icons.Material.Filled.Check;
            options?.Invoke(uiMessageOptions);
            
            var dialogOptions = CreateDefaultOptions();

            var parameters = new DialogParameters
            {
                { "UiMessageOptions", uiMessageOptions },
                { "ContentText", message },
                { "Color", Color.Success }
            };

            DialogService.Show<UiMessage>(title, parameters, dialogOptions);

            return Task.CompletedTask;
        }

        public Task Warn(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            var uiMessageOptions = CreateDefaultUiMessageOptions();
            uiMessageOptions.MessageIcon = Icons.Material.Filled.Warning;
            options?.Invoke(uiMessageOptions);

            var dialogOptions = CreateDefaultOptions();
            
            var parameters = new DialogParameters
            {
                { "UiMessageOptions", uiMessageOptions },
                { "ContentText", message },
                { "Color", Color.Warning }
            };

            DialogService.Show<UiMessage>(title, parameters, dialogOptions);

            return Task.CompletedTask;
        }

        public Task Error(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            var uiMessageOptions = CreateDefaultUiMessageOptions();
            uiMessageOptions.MessageIcon = Icons.Material.Filled.Error;
            options?.Invoke(uiMessageOptions);

            var dialogOptions = CreateDefaultOptions();
            
            var parameters = new DialogParameters
            {
                { "UiMessageOptions", uiMessageOptions },
                { "ContentText", message },
                { "Color", Color.Error }
            };

            DialogService.Show<UiMessage>(title, parameters, dialogOptions);

            return Task.CompletedTask;
        }

        public async Task<bool> Confirm(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            var uiMessageOptions = CreateDefaultUiMessageOptions();
            uiMessageOptions.MessageIcon = Icons.Material.Filled.Feedback;
            options?.Invoke(uiMessageOptions);
            
            var dialogOptions = CreateDefaultOptions();

            var parameters = new DialogParameters
            {
                { "UiMessageOptions", uiMessageOptions },
                { "ContentText", message },
                { "Color", Color.Error },
                { "IsConfirm", true }
            };

            var dialog = DialogService.Show<UiMessage>(title, parameters, dialogOptions);
            var result = await dialog.Result;

            return !result.Canceled;
        }

        private DialogOptions CreateDefaultOptions()
        {
            return new DialogOptions
            {
                CloseButton = true,
                Position = DialogPosition.Center,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true,
            };
        }

        private UiMessageOptions CreateDefaultUiMessageOptions()
        {
            return new UiMessageOptions
            {
                CenterMessage = true,
                ShowMessageIcon = true,
                OkButtonText = _localizer["Ok"],
                CancelButtonText = _localizer["Cancel"],
                ConfirmButtonText = _localizer["Yes"],
            };
        }
    }
}