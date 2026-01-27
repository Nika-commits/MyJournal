using Microsoft.AspNetCore.Components;
using MyJournal.Models;
using MyJournal.Services.Interfaces;

namespace MyJournal.Components.Pages
{
    public partial class MyJournals : ComponentBase
    {
        [Inject]
        public IDatabaseService DbService { get; set; } = default!;

        [Inject]
        public NavigationManager NavManager { get; set; } = default!;

        public List<Journal> JournalList { get; set; } = new();

        private bool showDeleteDialog = false;

        private Journal? entryToDelete;

        private string searchTerm = string.Empty;
        private string filterCategory = "All";
        private string filterTag = "All";

        private int CurrentPage = 1;
        private int PageSize = 10;
        private int TotalEntries = 0;
        private int TotalPages => (int)Math.Ceiling((double)TotalEntries / PageSize);
        private bool CanGoBack => CurrentPage > 1;
        private bool CanGoNext => CurrentPage < TotalPages;
        protected override async Task OnInitializedAsync()
        {
            await LoadJournals();
        }

        private async Task ExecuteSearch()
        {
            if(string.IsNullOrWhiteSpace(searchTerm) && filterCategory =="All" && filterTag == "All")
            {
                CurrentPage = 1;
                await LoadJournals();
            }
            else
            {
                JournalList = await DbService.SearchJournalsAsync(searchTerm, filterCategory, filterTag);
                TotalEntries = 0;
            }
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
            TotalEntries = await DbService.GetTotalCountAsync();
            int skip = (CurrentPage - 1) * PageSize;
            JournalList = await DbService.GetJournalsPaginatedAsync(skip, PageSize);

        }

        private async Task NextPage()
        {
            if (CanGoNext)
            {
                CurrentPage++;
                await LoadJournals();
            }
        }

        private async Task PreviousPage()
        {
            if (CanGoBack)
            {
                CurrentPage--;
                await LoadJournals();
            }
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