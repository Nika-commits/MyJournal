using System.Collections.Generic;
using System.Linq;

namespace MyJournal.Services
{
    public class MoodItem
    {
        public string Name { get; set; } = "";
        public string Emoji { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public static class MoodHelperService
    {
        public static List<string> PredefinedTags = new()
        {
            "Work", "Career", "Studies", "Family", "Friends", "Relationships",
            "Health", "Fitness", "Personal Growth", "Self-care", "Hobbies", "Travel", "Nature",
            "Finance", "Spirituality", "Birthday", "Holiday", "Vacation", "Celebration", "Exercise",
            "Reading", "Writing", "Cooking", "Meditation", "Yoga", "Music", "Shopping", "Parenting",
            "Projects", "Planning", "Reflection"
        };


        public static List<MoodItem> AllMoods = new()
        {
            new MoodItem { Name = "Happy", Emoji = "😊", Category = "Positive" },
            new MoodItem { Name = "Excited", Emoji = "🤩", Category = "Positive" },
            new MoodItem { Name = "Relaxed", Emoji = "😌", Category = "Positive" },
            new MoodItem { Name = "Grateful", Emoji = "🙏", Category = "Positive" },
            new MoodItem { Name = "Confident", Emoji = "😎", Category = "Positive" },

            new MoodItem { Name = "Calm", Emoji = "🍃", Category = "Neutral" },
            new MoodItem { Name = "Thoughtful", Emoji = "🤔", Category = "Neutral" },
            new MoodItem { Name = "Curious", Emoji = "🧐", Category = "Neutral" },
            new MoodItem { Name = "Nostalgic", Emoji = "🌅", Category = "Neutral" },
            new MoodItem { Name = "Bored", Emoji = "😑", Category = "Neutral" },

            new MoodItem { Name = "Sad", Emoji = "😢", Category = "Negative" },
            new MoodItem { Name = "Angry", Emoji = "😡", Category = "Negative" },
            new MoodItem { Name = "Stressed", Emoji = "😫", Category = "Negative" },
            new MoodItem { Name = "Lonely", Emoji = "🥀", Category = "Negative" },
            new MoodItem { Name = "Anxious", Emoji = "😰", Category = "Negative" }
        };

        public static string GetEmoji(string moodName)
        {
            var mood = AllMoods.FirstOrDefault(m => m.Name == moodName);
            return mood?.Emoji ?? "😐";
        }
    }
}