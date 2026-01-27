using Microsoft.AspNetCore.Components;
using MyJournal.Services.Interfaces;
using MyJournal.Services;

namespace MyJournal.Components.Shared
{
    public partial class Toast : IDisposable
    {
        private string Message { get; set; } = "";
        private string LevelClass { get; set; } = "toast-success";
        private bool IsVisible { get; set; }
        [Inject] public IToastService ToastService { get; set; } = default!;

        protected override void OnInitialized()
        {
            ToastService.OnShow += ShowToast;
            ToastService.OnHide += HideToast;
        }

        private void ShowToast(string message, ToastLevel level)
        {
            Message = message;
            LevelClass = $"toast-{level.ToString().ToLower()}";
           
            IsVisible = true;
            InvokeAsync(StateHasChanged);
        }

        private void HideToast()
        {
            IsVisible = false;
            InvokeAsync(StateHasChanged);
        }

        private void Close()
        {
            IsVisible = false;
        }

        public void Dispose()
        {
            ToastService.OnShow -= ShowToast;
            ToastService.OnHide -= HideToast;
        }
    }
}
