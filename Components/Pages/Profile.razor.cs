using Microsoft.AspNetCore.Components;
using MyJournal.Models;
using MyJournal.Services.Interfaces;


namespace MyJournal.Components.Pages
{
    public partial class Profile
    {
        private JournalStats? Stats;
        private string MemberSince = "Recently";
        [Inject] public IDatabaseService DbService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Stats = await DbService.GetJournalStatsAsync();

            var allJournals = await DbService.GetJournalsAsync();
            var firstEntry = allJournals.LastOrDefault(); 

            if (firstEntry != null)
            {
                MemberSince = firstEntry.EntryDate.ToString("MMMM yyyy");
            }
        }
    }
}
