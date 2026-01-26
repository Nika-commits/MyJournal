using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Services
{
    public class ThemeService
    {
        private const string ThemeKey = "IsDarkMode";
        public event Action? OnThemeChanged;

        public bool IsDarkMode { get; set; }

        public ThemeService()
        {
            IsDarkMode = Preferences.Get(ThemeKey, false);
        }

        public void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            Preferences.Set(ThemeKey, IsDarkMode);
            OnThemeChanged?.Invoke();
        }
    }
}
