using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyJournal.Components.Models;
using MyJournal.Services;

namespace MyJournal.Components.Pages
{
    public partial class TodaysJournal : ComponentBase
    {
        [Inject] public DatabaseService DbService { get; set; } = default!;
        [Inject] public NavigationManager NavManager { get; set; } = default!;
        [Inject] public ToastService Toast { get; set; } = default!;

        public Journal CurrentEntry { get; set; } = new Journal();

        private bool isLoaded = false;
        private bool IsEditorInitialized = false;

        private string selectedCategory = "Positive";
        private List<string> selectedTags = new();
        //public List<string> MoodOptions = ["Happy", "Excited", "Calm", "Sad", "Stressed", "Angry"];

        public void SetPrimaryMood(MoodItem mood)
        {
            CurrentEntry.PrimaryMood = mood.Name;
            CurrentEntry.MoodCategory = mood.Category;

        }
        public void ToggleSecondaryMood(MoodItem mood)
        {
            var list = CurrentEntry.SecondaryMoodList;

            if (list.Contains(mood.Name))
            {
                list.Remove(mood.Name); 
            }
            else
            {
                if (list.Count < 2) 
                {
                    list.Add(mood.Name);
                }
                else
                {
                    Toast.ShowToast("You can only select 2 secondary moods", ToastLevel.Warning);
                    return;
                }
            }
            CurrentEntry.SecondaryMoodList = list;
        }

        protected override async Task OnInitializedAsync()
        {
            var existing = await DbService.GetTodaysJournalAsync();

            if (existing != null) CurrentEntry = existing;
            else
            {
                CurrentEntry = new Journal
                {
                    EntryDate = DateTime.Today,
                    Id = Guid.NewGuid(), 
                    Title = "",
                    Content = "",
                };


            }
            isLoaded = true;

            if (!string.IsNullOrEmpty(CurrentEntry.Tags)) selectedTags = CurrentEntry.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!string.IsNullOrEmpty(CurrentEntry.MoodCategory)) selectedCategory = CurrentEntry.MoodCategory;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isLoaded && !IsEditorInitialized)
            {
                IsEditorInitialized = true;

                await JS.InvokeVoidAsync("initQuill", "editor-container");

                if (!string.IsNullOrEmpty(CurrentEntry.Content))
                {
                    await JS.InvokeVoidAsync("setQuillHtml", CurrentEntry.Content);
                }
            }
        }

        public void SelectCategory(string category)
        {
            selectedCategory = category;
        }

        public void ToggleTag(string tag)
        {
            if (selectedTags.Contains(tag)) selectedTags.Remove(tag);
            else selectedTags.Add(tag);

            CurrentEntry.Tags = string.Join(",", selectedTags);
        }

        //public void SetMood(string mood)
        //{
        //    CurrentEntry.Mood = mood;
        //}


        public async Task SaveEntry()
        {
            if (string.IsNullOrWhiteSpace(CurrentEntry.Title)) return;

            CurrentEntry.Content = await JS.InvokeAsync<string>("getQuillHtml");
            CurrentEntry.UpdatedAt = DateTime.UtcNow;

            await DbService.SaveJournalAsync(CurrentEntry);
            StateHasChanged();

            Toast.ShowToast($"Saved at {DateTime.Now:HH:mm}");
        }

        public void Cancel()
        {
            NavManager.NavigateTo("/");
        }
    }
}