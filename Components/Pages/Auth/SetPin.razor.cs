using Microsoft.AspNetCore.Components;


namespace MyJournal.Components.Pages.Auth
{
    public partial class SetPin
    {
        string pin = "";
        string errorMessage = "";

        async Task SavePin()
        {
            if (string.IsNullOrWhiteSpace(pin))
            {
                errorMessage = "Cannot be empty";
                return;
            }

                await Auth.SetPinAsync(pin);
            await OnSuccess.InvokeAsync();
        }
        [Parameter] public EventCallback OnSuccess { get; set; }
    }
}
