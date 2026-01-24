using Microsoft.AspNetCore.Components;


namespace MyJournal.Components.Pages.Auth
{
    public partial class AuthGuard
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }

        private bool isPinSet;

        protected override async Task OnInitializedAsync()
        {
            isPinSet = await Auth.IsPinSetAsync();
        }

        void Unlock()
        {
            Session.IsUnlocked = true;
        }

        void OnPinCreated()
        {
            isPinSet = true;
            Session.IsUnlocked = true;
        }

    }
}