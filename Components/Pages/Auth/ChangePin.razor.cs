using Microsoft.AspNetCore.Components;
using MyJournal.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Pages.Auth
{
    public partial class ChangePin
    {
        private bool isVerified = false;
        [Inject] public ToastService Toast { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; } = default!;

        private void HandleVerified()
        {
            isVerified = true;
            StateHasChanged();
        }

    private void HandlePinChanged()
        {
            Toast.ShowToast("PIN updated successfully", ToastLevel.Success);
            Nav.NavigateTo("/settings");
        }

        private void Cancel()
        {
            Nav.NavigateTo("/settings");
        }
    }
}
