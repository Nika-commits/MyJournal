using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Models
{
    public class JournalStats
    {
        public int TotalEntries { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int WordsWritten { get; set; }
        public string TopMood { get; set; } = "";


    }
}
