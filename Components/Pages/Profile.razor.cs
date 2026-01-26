using MyJournal.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Pages
{
    public partial class Profile
    {
        private JournalStats? Stats;
        private string MemberSince = "Recently";

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
