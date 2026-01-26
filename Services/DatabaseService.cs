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

        public async Task<List<Journal>> GetJournalsPaginatedAsync(int skip, int take)
        {
            await InitAsync();
            return await _database!.Table<Journal>().OrderByDescending(j => j.EntryDate).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            await InitAsync();
            return await _database!.Table<Journal>().CountAsync();
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


        public async Task<List<Journal>> SearchJournalsAsync(string keyword, string? category = null, string? tag = null)
        {
            await InitAsync();
            var query = _database!.Table<Journal>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(j => j.Title.Contains(keyword) || j.Content.Contains(keyword));
            }

            if(!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                query = query.Where(j => j.MoodCategory == category);
            }

            if (!string.IsNullOrWhiteSpace(tag) && tag!= "All")
            {
                query = query.Where(j => j.Tags.Contains(tag));
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

        public async Task<List<JournalMetadata>> GetJournalMetadataAsync()
        {
            await InitAsync();
            var query = _database!.Table<Journal>();
            var all = await query.ToListAsync();

            return all.Select(j => new JournalMetadata
            {
                Id = j.Id,
                EntryDate = j.EntryDate,
                Mood = j.PrimaryMood
            }).ToList();
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
                LongestStreak = 0,
                TopMood = "N/A"
            };

            if (!allEntries.Any()) return stats;

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

            var moodGroups = allEntries.Where(e => !string.IsNullOrEmpty(e.PrimaryMood))
                                      .GroupBy(e => e.PrimaryMood)
                                      .OrderByDescending(g => g.Count())
                                      .FirstOrDefault();

            if (moodGroups != null) stats.TopMood = moodGroups.Key;

            var moodCounts = new Dictionary<string, int>();

            foreach (var entry in allEntries)
            {
                if (!string.IsNullOrEmpty(entry.MoodCategory))
                {
                    if (moodCounts.ContainsKey(entry.MoodCategory)) moodCounts[entry.MoodCategory]++;
                    else moodCounts[entry.MoodCategory] = 1;
                }
                else if (!string.IsNullOrEmpty(entry.PrimaryMood))
                {
                    var foundCategory = MoodHelperService.AllMoods.FirstOrDefault(m => m.Name == entry.PrimaryMood)?.Category;
                    if (!string.IsNullOrEmpty(foundCategory))
                    {
                        if (moodCounts.ContainsKey(foundCategory)) moodCounts[foundCategory]++;
                        else moodCounts[foundCategory] = 1;
                    }
                }
            }
            stats.MoodCounts = moodCounts;

            var tagDict = new Dictionary<string, int>();
            foreach (var entry in allEntries)
            {
                if (string.IsNullOrEmpty(entry.Tags)) continue;
                foreach (var t in entry.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    var cleanTag = t.Trim();
                    if (tagDict.ContainsKey(cleanTag)) tagDict[cleanTag]++;
                    else tagDict[cleanTag] = 1;
                }
            }

            stats.TopTags = tagDict.OrderByDescending(x => x.Value).Take(10).ToDictionary(x => x.Key, x => x.Value);

            var dayGroup = allEntries.GroupBy(e => e.EntryDate.DayOfWeek).OrderByDescending(g => g.Count()).FirstOrDefault();
            if (dayGroup != null) stats.MostProductiveDay = dayGroup.Key.ToString();

            return stats;

        }

    }


}