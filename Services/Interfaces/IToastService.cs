using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Services.Interfaces
{
    public interface IToastService
    {
        event Action<string, ToastLevel>? OnShow;
        event Action? OnHide;

        void ShowToast(string message, ToastLevel level = ToastLevel.Success);

        void StartCountdown();
    }
}
