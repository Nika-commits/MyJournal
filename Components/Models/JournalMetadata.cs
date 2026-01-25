using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Models
{
    public class JournalMetadata
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Mood { get; set; } = "";
    }
}
