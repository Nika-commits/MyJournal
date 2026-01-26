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

        public List<Journal> JournalList { get; set; } = new();

        private bool showDeleteDialog = false;

        private Journal? entryToDelete;

        private string searchTerm = string.Empty;
        private string filterCategory = "All";
        private string filterTag = "All";
        protected override async Task OnInitializedAsync()
        {
            await LoadJournals();
        }

        private async Task ExecuteSearch()
        {
            JournalList = await DbService.SearchJournalsAsync(searchTerm, filterCategory, filterTag);
        }

        private async Task OnCategoryChanged(ChangeEventArgs e)
        {
            filterCategory = e.Value?.ToString() ?? "All";
            await ExecuteSearch();
        }
        private async Task OnTagChanged(ChangeEventArgs e)
        {
            filterTag = e.Value?.ToString() ?? "All";
            await ExecuteSearch();
        }

        private async Task LoadJournals()
        {
            JournalList = await DbService.GetJournalsAsync();
        }

        public string GetPreview(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return string.Empty;

            var plainText = System.Text.RegularExpressions.Regex.Replace(content, "<.*?>", string.Empty);
            return plainText.Length > 100 ? plainText.Substring(0, 100) + "..." : plainText;
        }


        public void ViewEntry(Guid id)
        {
            NavManager.NavigateTo($"/viewJournal/{id}");
        }
      

        public void RequestDeleteEntry(Journal entry)
        {
            entryToDelete = entry;
            showDeleteDialog = true;
        }

        public async Task HandleDeleteResult(bool confirmed)
        {
            showDeleteDialog = false;
            if (confirmed && entryToDelete != null)
            {
                await DbService.DeleteJournalAsync(entryToDelete.Id);
                await LoadJournals();
            }
            entryToDelete = null;
        }
    }
}