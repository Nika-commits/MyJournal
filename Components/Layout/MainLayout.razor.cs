using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Layout
{
    public partial class MainLayout
    {
        protected override void OnInitialized()
        {
            // 4. Subscribe to Theme Changes so the UI updates instantly
            Theme.OnThemeChanged += StateHasChanged;
        }

        public void Dispose()
        {
            Theme.OnThemeChanged -= StateHasChanged;
        }
    }
}
