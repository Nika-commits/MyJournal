using MyJournal.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Shared
{
    public partial class Toast
    {
        private string Message { get; set; } = "";
        private string LevelClass { get; set; } = "toast-success";
        private bool IsVisible { get; set; }

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
