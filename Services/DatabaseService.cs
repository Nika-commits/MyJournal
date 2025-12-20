using MyJournal.Components.Models; // Imports your Journal class
using SQLite;

namespace MyJournal.Services
{
    public class DatabaseService
    {
        // The actual connection to the file
        private SQLiteAsyncConnection? _database;

        // 1. Initialize the Database (Create file & table)
        async Task Init()
        {
            if (_database is not null)
                return;

            // Get the path to the 'safe' folder on the device
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyJournal.db3");

            // Open the connection
            _database = new SQLiteAsyncConnection(dbPath,
                SQLiteOpenFlags.ReadWrite |
                SQLiteOpenFlags.Create |
                SQLiteOpenFlags.SharedCache);

            // Create the table based on your 'Journal' blueprint
            await _database.CreateTableAsync<Journal>();
        }

        // 2. GET: Fetch all journals
        public async Task<List<Journal>> GetJournalsAsync()
        {
            await Init();
            // Get all items and sort them by Date (newest first)
            return await _database!.Table<Journal>()
                                  .OrderByDescending(x => x.EntryDate)
                                  .ToListAsync();
        }

        // 3. SAVE: Add or Update a journal
        public async Task<int> SaveJournalAsync(Journal item)
        {
            await Init();

            // If the ID is new/empty, Insert. Otherwise, Update.
            // Note: Since you used Guid, we check if the row exists differently.
            // For simplicity with Guids, we usually just try InsertOrReplace, 
            // or check if we can find it first.

            return await _database!.InsertOrReplaceAsync(item);
        }

        // 4. DELETE: Remove a journal
        public async Task<int> DeleteJournalAsync(Journal item)
        {
            await Init();
            return await _database!.DeleteAsync(item);
        }
    }
}