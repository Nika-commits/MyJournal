using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Pages.Auth
{
    public partial class UnlockPin
    {
        string pin = "";
        string message = "";
        [Parameter] public EventCallback OnSuccess { get; set; }

        async Task Unlock()
        {
            if (await Auth.IsLockedOutAsync())
            {
                 (int _, TimeSpan remaining) = await Auth.GetLockOutStatusAsync();
                message = $"Device is locked. Try again in {remaining.TotalSeconds:N0} seconds.";
                return;

            }
            if(await Auth.ValidatePinAsync(pin))
            {
                await OnSuccess.InvokeAsync();
            }
            else
            {
                (int currentAttempts, TimeSpan _) = await Auth.GetLockOutStatusAsync();
               int attemptsLeft = Auth.MaxAllowedAttempts - currentAttempts;
                message = $"Incorrect Pin, {attemptsLeft} attempts remaining";
            }
            pin = "";
        }

        private async Task HandleKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await Unlock();
            }
        }


    }

}
