using Microsoft.AspNetCore.Components;
using MyJournal.Components.Models;
using MyJournal.Services;


namespace MyJournal.Components.Pages
{
    public partial class Home
    {
        private JournalStats? Stats;

        protected override async Task OnInitializedAsync()
        {
            Stats = await DbService.GetJournalStatsAsync();
        }
    }
}
