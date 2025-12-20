using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyJournal.Components.Models
{
    public class Journal
    {
        [PrimaryKey]
        [AutoIncrement]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Mood { get; set; }
        public bool IsTrashed { get; set; }
        public bool IsFavorite { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime EntryDate { get; set; }
        public Journal()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
