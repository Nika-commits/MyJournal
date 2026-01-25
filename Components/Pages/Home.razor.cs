using Microsoft.AspNetCore.Components;
using MyJournal.Services;


namespace MyJournal.Components.Pages
{
    public partial class Home
    {
        [Inject] public ToastService Toast { get; set; } = default!;
        public async Task ToastDemo()
        {
            Console.WriteLine("Pressed 1");
            Toast.ShowToast("Success!", ToastLevel.Success);

            await Task.Delay(2000); // Wait 2 seconds

            Toast.ShowToast("Error!", ToastLevel.Error);
            Console.WriteLine("Pressed 2");
        }
    }
}
