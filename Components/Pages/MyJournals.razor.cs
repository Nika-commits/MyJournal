using Microsoft.AspNetCore.Components;
using MyJournal.Components.Models;
using MyJournal.Services;

namespace MyJournal.Components.Pages
{
    public partial class MyJournals : ComponentBase
    {
        [Inject]
        public DatabaseService DbService { get; set; } = default!;

        [Inject]
        public NavigationManager NavManager { get; set; } = default!;

        // The list that holds our data
        public List<Journal> JournalList { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            // Fetch data from SQLite
            JournalList = await DbService.GetJournalsAsync();
        }

        // Helper to convert mood text to emoji
        public string GetMoodEmoji(string mood) => mood switch
        {
            "Happy" => "😊",
            "Excited" => "🤩",
            "Calm" => "😌",
            "Sad" => "😢",
            "Stressed" => "😫",
            "Angry" => "😡",
            _ => "😐"
        };

        // Feature: Delete an entry
        public async Task DeleteEntry(Journal entry)
        {
            bool confirm = await Application.Current!.MainPage!.DisplayAlert(
                "Delete Entry",
                "Are you sure you want to delete this?",
                "Yes",
                "No");

            if (confirm)
            {
                await DbService.DeleteJournalAsync(entry);
                // Refresh the list
                JournalList = await DbService.GetJournalsAsync();
            }
        }
    }
}