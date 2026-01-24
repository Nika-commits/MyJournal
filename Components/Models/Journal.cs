
using SQLite;

namespace MyJournal.Components.Models
{
    public class Journal
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Mood { get; set; }
        public bool IsTrashed { get; set; }
        public bool IsFavorite { get; set; }
        public string? Tags { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Today;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public  int WordCount { get; set; }
    }
}
