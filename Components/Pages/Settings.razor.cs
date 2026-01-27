using MyJournal.Services;
using Microsoft.AspNetCore.Components;
using MyJournal.Services.Interfaces;

namespace MyJournal.Components.Pages
{
    public partial class Settings
    {
        [Inject] public ThemeService Theme { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; } = default!;
        [Inject] public IToastService Toast { get; set; } = default!;

        protected override void OnInitialized()
        {
            Theme.OnThemeChanged += StateHasChanged;
        }

        private void ToggleTheme()
        {
            Theme.ToggleTheme();
            Toast.ShowToast(Theme.IsDarkMode?"Dark Mode Activated" : "Light Mode Activated", ToastLevel.Success);
            
        }

        private void GoToProfile()
        {
            Nav.NavigateTo("/profile");
        }
        private void GoToChangePin()
        {
            Nav.NavigateTo("/change-pin");
        }

        public void Dispose()
        {
            Theme.OnThemeChanged -= StateHasChanged;
        }
    }
}
