namespace ProcrastiInfrastructure.Models
{
    public class AchievementItemViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsUnlocked { get; set; }
    }
    public class AchievementsViewModel
    {
        public int EarnedCount { get; set; }
        public int TotalCount { get; set; }
        public List<AchievementItemViewModel> Achievements { get; set; }
    }
}
