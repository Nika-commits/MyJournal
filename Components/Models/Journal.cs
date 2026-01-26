
using SQLite;

namespace MyJournal.Components.Models
{
    public class Journal
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string PrimaryMood { get; set; } = string.Empty;
        public string SecondaryMoods { get; set; } = string.Empty;
        public string MoodCategory { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;


        public bool IsTrashed { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Today;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public  int WordCount { get; set; }

        [Ignore]
        public List <string> SecondaryMoodList
        {
            get => string.IsNullOrWhiteSpace(SecondaryMoods) ? new List<string>() : SecondaryMoods.Split(',').ToList();
            set => SecondaryMoods = string.Join(',', value);
        }
    }
}
