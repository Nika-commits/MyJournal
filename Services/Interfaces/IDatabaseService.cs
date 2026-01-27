using MyJournal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<List<Journal>> GetJournalsAsync();
        Task<Journal> GetJournalByIdAsync(Guid id);
        Task<int> SaveJournalAsync(Journal item);
        Task<int> DeleteJournalAsync(Guid id);

        Task<List<Journal>> GetJournalsPaginatedAsync(int skip, int take);
        Task<int> GetTotalCountAsync();
        Task<List<Journal>> SearchJournalsAsync(string keyword, string? category = null, string? tag = null);

        Task<Journal> GetEntryByDateAsync(DateTime date);
        Task<Journal?> GetTodaysJournalAsync();
        Task<List<DateTime>> GetEntryDatesAsync();

        Task<List<JournalMetadata>> GetJournalMetadataAsync();
        Task<JournalStats> GetJournalStatsAsync();
    }
}
