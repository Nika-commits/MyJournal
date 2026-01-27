
using MyJournal.Services.Interfaces;

namespace MyJournal.Services
{
    public partial class ToastService : IToastService ,IDisposable
    {
        public event Action<string, ToastLevel>? OnShow;
        public event Action? OnHide;

        public System.Timers.Timer? _countdown;
        public void ShowToast(string message, ToastLevel level = ToastLevel.Success)
        {
            OnShow?.Invoke(message, level);
            StartCountdown();
        }

        public void StartCountdown()
        {
            SetCountdown();
            if (_countdown!.Enabled)
            {
                _countdown.Stop();
                _countdown.Start();
            }
            else
            {
                _countdown.Start();
            }
        }

        private void SetCountdown()
        {
            if (_countdown == null)
            {
                _countdown = new System.Timers.Timer(3000);
                _countdown.Elapsed += HideToast;
                _countdown.AutoReset = false;
            }
        }

        public void HideToast(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnHide?.Invoke();
        }

        public void Dispose()
        {
           _countdown?.Dispose();
        }
    }
    public enum ToastLevel
    {
        Success,
        Info,
        Warning,
        Error
    }
}
