using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyJournal.Components.Models;
using MyJournal.Services;

namespace MyJournal.Components.Pages
{
    public partial class CreateJournal : ComponentBase
    {
        [Inject]
        public DatabaseService DbService { get; set; } = default!;

        [Inject]
        public NavigationManager NavManager { get; set; } = default!;

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
                    Id = Guid.NewGuid()
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
        public static string GetMoodEmoji(string mood) => mood switch
        {
            "Happy" => "😊",
            "Excited" => "🤩",
            "Calm" => "😌",
            "Sad" => "😢",
            "Stressed" => "😫",
            "Angry" => "😡",
            _ => "😐"
        };

        public void SetMood(string mood)
        {
            CurrentEntry.Mood = mood;
        }

        public async Task SaveEntry()
        {
            if (string.IsNullOrWhiteSpace(CurrentEntry.Title)) return;

            var HtmlContent = await JS.InvokeAsync<string>("getQuillHtml");

            var existing = await DbService.GetEntryByDateAsync(CurrentEntry.EntryDate);
            if (existing != null && existing.Id != CurrentEntry.Id) return;

            CurrentEntry.EntryDate = DateTime.UtcNow;
            CurrentEntry.UpdatedAt = DateTime.UtcNow;

            await DbService.SaveJournalAsync(CurrentEntry);
            NavManager.NavigateTo("/myjournals");
        }

        public void Cancel()
        {
            NavManager.NavigateTo("/");
        }
    }
}