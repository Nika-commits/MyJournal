using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyJournal.Components.Models;
using MyJournal.Services;

namespace MyJournal.Components.Pages
{
    public partial class TodaysJournal : ComponentBase
    {
        [Inject] public DatabaseService DbService { get; set; } = default!;

        [Inject] public NavigationManager NavManager { get; set; } = default!;
        [Inject] public ToastService Toast { get; set; } = default!;

        public Journal CurrentEntry { get; set; } = new Journal();
        public string StatusMessage { get; set; } = "";

        public List<string> MoodOptions = ["Happy", "Excited", "Calm", "Sad", "Stressed", "Angry"];
        protected override async Task OnInitializedAsync()
        {
            var existing = await DbService.GetTodaysJournalAsync();

            if (existing != null) CurrentEntry = existing;
            else
            {
                CurrentEntry = new Journal
                {
                    EntryDate = DateTime.Today,
                    Id = Guid.NewGuid(), 
                    Title = "",
                    Content = "",
                };


            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("initQuill", "editor-container");

                if (!string.IsNullOrEmpty(CurrentEntry.Content))
                {
                    await JS.InvokeVoidAsync("setQuillHtml", CurrentEntry.Content);
                }
            }
        }

        public void SetMood(string mood)
        {
            CurrentEntry.Mood = mood;
        }

        public async Task SaveEntry()
        {
            if (string.IsNullOrWhiteSpace(CurrentEntry.Title)) return;

            CurrentEntry.Content = await JS.InvokeAsync<string>("getQuillHtml");
            CurrentEntry.UpdatedAt = DateTime.UtcNow;

            await DbService.SaveJournalAsync(CurrentEntry);
            StatusMessage = $"Journal saved at {DateTime.Now:HH:mm}";
            StateHasChanged();

            Toast.ShowToast($"Saved at {DateTime.Now:HH:mm}");
        }

        public void Cancel()
        {
            NavManager.NavigateTo("/");
        }
    }
}