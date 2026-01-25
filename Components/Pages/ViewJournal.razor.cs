using Microsoft.AspNetCore.Components;
using MyJournal.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Pages
{
    public partial class ViewJournal
    {
        [Parameter] public Guid Id { get; set; }
        private Journal? Entry;

        protected override async Task OnInitializedAsync()
        {
            Entry = await DbService.GetJournalByIdAsync(Id);
        }

        private void GoBack() => NavManager.NavigateTo("/myJournals");

        // Feature for later: Re-use your TodaysJournal page but populate it with this ID
        // For now, we just keep it view-only.
        private void GoToEdit()
        {
            // Logic to edit can be added later if needed
        }
    }
}
