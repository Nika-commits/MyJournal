using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Services
{
    public static class MoodHelperService
    {
        public static string GetEmojisPerMood(string mood) => mood switch
        {
            "Happy" => "😊",
            "Excited" => "🤩",
            "Calm" => "😌",
            "Sad" => "😢",
            "Stressed" => "😫",
            "Angry" => "😡",
            _ => "😐"
        };
    }
}
