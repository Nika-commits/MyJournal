using Microsoft.AspNetCore.Components;
using MyJournal.Models;
using MyJournal.Services.Interfaces;


namespace MyJournal.Components.Pages.Auth
{
    public partial class AuthGuard
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Inject] public IAuthService Auth { get; set; } = default!;
        [Inject] public AuthSession  Session { get; set; } = default!;

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