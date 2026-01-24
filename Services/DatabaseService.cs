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

        public async Task<List<Journal>> SearchJournalsAsync(string keyword, string? selectedMood =null)
        {
            await InitAsync();
            var query = _database!.Table<Journal>();
            if(!string.IsNullOrWhiteSpace(keyword))
                {
                query = query.Where(j => j.Title.Contains(keyword) || j.Content.Contains(keyword));
            }
            if(!string.IsNullOrWhiteSpace(selectedMood) && selectedMood != "All")
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
    }


}