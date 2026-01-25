using MyJournal.Components.Models;
using SQLite;

namespace MyJournal.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _initialized;

        private async Task InitAsync()
        {
            if (_initialized) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyJournalDatabase.db");

            _database = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            _initialized = true;

            await _database.CreateTableAsync<Journal>();

        }

        public async Task<List<Journal>> GetJournalsAsync()
        {
            await InitAsync();
            return await _database!.Table<Journal>().OrderByDescending(j => j.EntryDate).ToListAsync();
        }

        public async Task<Journal> GetJournalByIdAsync(Guid id)
        {
            await InitAsync();
            return await _database!.Table<Journal>().Where(j => j.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteJournalAsync(Guid id)
        {
            await InitAsync();
            return await _database!.DeleteAsync<Journal>(id);
        }

        public async Task<int> SaveJournalAsync(Journal item)
        {
            await InitAsync();
            return await _database!.InsertOrReplaceAsync(item);
        }


        public async Task<List<Journal>> SearchJournalsAsync(string keyword, string? selectedMood = null)
        {
            await InitAsync();
            var query = _database!.Table<Journal>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(j => j.Title.Contains(keyword) || j.Content.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(selectedMood) && selectedMood != "All")
            {
                query = query.Where(j => j.Mood == selectedMood);
            }
            return await query.OrderByDescending(j => j.EntryDate).ToListAsync();
        }

        public async Task<Journal> GetEntryByDateAsync(DateTime date)
        {
            await InitAsync();
            var start = date.Date;
            var end = date.Date.AddDays(1);
            return await _database!.Table<Journal>().Where(j => j.EntryDate >= start && j.EntryDate < end).FirstOrDefaultAsync();
        }

        public async Task<Journal?> GetTodaysJournalAsync()
        {
            await InitAsync();
            var startOfDay = DateTime.Today;
            var endOfDay = startOfDay.AddDays(1);

            return await _database.Table<Journal>().Where(j => j.EntryDate >= startOfDay && j.EntryDate < endOfDay).FirstOrDefaultAsync();
        }

        public async Task<List<DateTime>> GetEntryDatesAsync()
        {
            await InitAsync();
            var journals = await _database!.Table<Journal>().ToListAsync();
            return journals.Select(j => j.EntryDate.Date).ToList();
        }

        public async Task<JournalStats> GetJournalStatsAsync()
        {
            await InitAsync();

            var allEntries = await _database.Table<Journal>().OrderByDescending(j => j.EntryDate).ToListAsync();

            var stats = new JournalStats
            {
                TotalEntries = allEntries.Count,
                WordsWritten = allEntries.Sum(e => string.IsNullOrEmpty(e.Content) ? 0 : e.Content.Split(' ').Length),
                CurrentStreak = 0,
                LongestStreak = 0
            };

            if (!allEntries.Any())
                return stats;

            var uniqueDates = allEntries
                .Select(e => e.EntryDate.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            var today = DateTime.Today;

            if (uniqueDates.Contains(today) || uniqueDates.Contains(today.AddDays(-1)))
            {
                int currentStreak = 1;

                var lastDate = uniqueDates.Contains(today) ? today : today.AddDays(-1);
                for (int i = 1; i < uniqueDates.Count; i++)
                {
                    if (uniqueDates[i] == lastDate) continue;

                    if (uniqueDates[i] == lastDate.AddDays(-1))
                    {
                        currentStreak++;
                        lastDate = uniqueDates[i];
                    }
                    else
                    {
                        break;
                    }
                }
                stats.CurrentStreak = currentStreak;

            }

            var ascendingDates = uniqueDates.OrderBy(d => d).ToList();
            int maxStreak = 0;
            int tempStreak = 1;

            for (int i = 1; i < ascendingDates.Count; i++)
            {
                if (ascendingDates[i] == ascendingDates[i - 1].AddDays(1))
                {
                    tempStreak++;
                }
                else
                {
                    if (tempStreak > maxStreak)
                    {
                        maxStreak = tempStreak;
                    }
                    tempStreak = 1;
                }
            }
            if (tempStreak > maxStreak)
            {
                maxStreak = tempStreak;
            }
            stats.LongestStreak = maxStreak;

            var moodGroups = allEntries.Where(e => !string.IsNullOrEmpty(e.Mood))
                                      .GroupBy(e => e.Mood)
                                      .OrderByDescending(g => g.Count())
                                      .FirstOrDefault();

            if (moodGroups != null) stats.TopMood = moodGroups.Key;
            return stats;

        }

    }


}