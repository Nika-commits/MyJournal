using MyJournal.Components.Models; 
using SQLite;

namespace MyJournal.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        async Task Init()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyJournal.db3");

            _database = new SQLiteAsyncConnection(dbPath,
                SQLiteOpenFlags.ReadWrite |
                SQLiteOpenFlags.Create |
                SQLiteOpenFlags.SharedCache);

            await _database.CreateTableAsync<Journal>();
        }

        public async Task<List<Journal>> GetJournalsAsync()
        {
            await Init();
            return await _database!.Table<Journal>()
                                  .OrderByDescending(x => x.EntryDate)
                                  .ToListAsync();
        }

        public async Task<int> SaveJournalAsync(Journal item)
        {
            await Init();
            return await _database!.InsertOrReplaceAsync(item);
        }

        public async Task<int> DeleteJournalAsync(Journal item)
        {
            await Init();
            return await _database!.DeleteAsync(item);
        }
    }
}