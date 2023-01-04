using System;
using MicroserviceDemo.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using MudBlazor.ThemeManager;

namespace MicroserviceDemo.Web.Shared
{
    public partial class MainLayout : IDisposable
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private ThemeManagerTheme _themeManager = new()
        {
            Theme = new MudBlazorAdminDashboard(),
            DrawerClipMode = DrawerClipMode.Never,
            FontFamily = "Montserrat",
            DefaultBorderRadius = 6,
            AppBarElevation = 1,
            DrawerElevation = 1
        };

        private bool _drawerOpen = true;
        private bool _themeManagerOpen;

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private void OpenThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        private void UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;

            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            _drawerOpen = true;

            StateHasChanged();
        }
    }
}