using Microsoft.AspNetCore.Components;
using MyJournal.Components.Models;
using MyJournal.Services;

namespace MyJournal.Components.Pages
{
    // FIX 2: Class name matches filename exactly (Singular)
    public partial class CreateJournal : ComponentBase
    {
        // FIX 3: Add '= default!;' to stop the "Non-nullable" warnings
        [Inject]
        public DatabaseService DbService { get; set; } = default!;

        [Inject]
        public NavigationManager NavManager { get; set; } = default!;

        public Journal CurrentEntry { get; set; } = new Journal();

        public List<string> MoodOptions = ["Happy", "Excited", "Calm", "Sad", "Stressed", "Angry"];
        protected override void OnInitialized()
        {
            // FIX 1: This works now because we updated the Model
            CurrentEntry.EntryDate = DateTime.Today;
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
            if (string.IsNullOrWhiteSpace(CurrentEntry.Title))
                return;

            CurrentEntry.CreatedAt = DateTime.UtcNow;
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