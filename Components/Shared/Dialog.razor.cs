using Microsoft.AspNetCore.Components;

namespace MyJournal.Components.Shared
{
    public partial class Dialog
    {
        [Parameter] public bool IsVisible { get; set; }
        [Parameter] public string Title { get; set; } = "Confirmation";
        [Parameter] public string Message { get; set; } = "Are you sure?";
        [Parameter] public string ConfirmButtonText { get; set; } = "Confirm";
        [Parameter] public string CancelButtonText { get; set; } = "Cancel";
        [Parameter] public string Type { get; set; } = "primary";
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }

    }
}
