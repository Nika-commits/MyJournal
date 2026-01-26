using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


namespace MyJournal.Components.Pages.Auth
{
    public partial class SetPin
    {
        string pin = "";
        string errorMessage = "";
        private ElementReference pinInput;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await pinInput.FocusAsync();
            }
        }
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
       
        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SavePin();
            }
        }
        [Parameter] public EventCallback OnSuccess { get; set; }
    }
}
