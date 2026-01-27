using Microsoft.AspNetCore.Components;
using MyJournal.Models;
using MyJournal.Services.Interfaces;


namespace MyJournal.Components.Pages
{
    public partial class Home
    {
        private JournalStats? Stats;
        [Inject] public IDatabaseService DbService { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            Stats = await DbService.GetJournalStatsAsync();
        }
    }
}
