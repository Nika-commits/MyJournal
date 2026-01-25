using Microsoft.AspNetCore.Components;
using MyJournal.Services;

namespace MyJournal.Components.Pages
{
    public partial class Calendar
    {
        [Inject] public DatabaseService DbService { get; set; } = default!;
        [Inject] public NavigationManager NavManager { get; set; } = default!;

        private DateTime displayDate = DateTime.Today;
        private List<DateTime> entryDates = new();
        private List<DateTime?> calendarDays = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadCalendar();
        }

        private async Task LoadCalendar()
        {
            entryDates = await DbService.GetEntryDatesAsync();
            GenerateCalendar();
        }

        private void GenerateCalendar()
        {
            calendarDays.Clear();
            var firstDayOfMonth = new DateTime(displayDate.Year, displayDate.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(displayDate.Year, displayDate.Month);

            // Calculate offset (0 for Sunday, 1 for Monday, etc.)
            int dayOffset = (int)firstDayOfMonth.DayOfWeek;

            // Fill empty slots for previous month
            for (int i = 0; i < dayOffset; i++)
            {
                calendarDays.Add(null);
            }

            // Fill actual days
            for (int i = 1; i <= daysInMonth; i++)
            {
                calendarDays.Add(new DateTime(displayDate.Year, displayDate.Month, i));
            }
        }

        private async Task ChangeMonth(int increment)
        {
            displayDate = displayDate.AddMonths(increment);
            await LoadCalendar();
        }

        private void HandleDateClick(DateTime date)
        {
            if (entryDates.Contains(date.Date))
            {
                // Logic: If entry exists, we need the ID to view it. 
                // Alternatively, search by date in the View page.
                NavManager.NavigateTo($"/myJournals?searchDate={date:yyyy-MM-dd}");
            }
        }
    }
}