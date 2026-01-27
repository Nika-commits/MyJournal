using Microsoft.AspNetCore.Components;
using MyJournal.Services;
using MyJournal.Models;

namespace MyJournal.Components.Pages
{
    public partial class Calendar
    {
        [Inject] public DatabaseService DbService { get; set; } = default!;
        [Inject] public NavigationManager NavManager { get; set; } = default!;

        private DateTime displayDate = DateTime.Today;

        private Dictionary<DateTime, JournalMetadata> entryMap = new();
        private List<DateTime?> calendarDays = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var rawList = await DbService.GetJournalMetadataAsync();

            // Create a lookup dictionary: Key = Date (Midnight), Value = Data
            entryMap = rawList.ToDictionary(k => k.EntryDate.Date, v => v);

            GenerateCalendar();
        }

        private void GenerateCalendar()
        {
            calendarDays.Clear();
            var firstDay = new DateTime(displayDate.Year, displayDate.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(displayDate.Year, displayDate.Month);

            // Adjust offset so grid starts correctly (0 = Sunday)
            int offset = (int)firstDay.DayOfWeek;

            for (int i = 0; i < offset; i++) calendarDays.Add(null);

            for (int i = 1; i <= daysInMonth; i++)
            {
                calendarDays.Add(new DateTime(displayDate.Year, displayDate.Month, i));
            }
        }

        private async Task ChangeMonth(int value)
        {
            displayDate = displayDate.AddMonths(value);
            GenerateCalendar();
        }

        private void OnDateClick(DateTime? date)
        {
            if (date == null) return;

            // Check if we have an entry for this specific date
            if (entryMap.ContainsKey(date.Value.Date))
            {
                var entry = entryMap[date.Value.Date];
                // Navigate directly to the View Page using the GUID
                NavManager.NavigateTo($"/viewJournal/{entry.Id}");
            }
            else if (date.Value.Date == DateTime.Today)
            {
                // Optional: If clicking Today but no entry exists, go to Create
                NavManager.NavigateTo("/todaysJournal");
            }
        }
    }
}