using ProcrastiDomain.Model;

namespace ProcrastiInfrastructure.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public string CurrentTitle { get; set; } = null!;
        public DateTime? JoinedDate { get; set; }
        
        public int AchievementsCount { get; set; }
        public int TitlesCount { get; set; }
        public int TotalLoss { get; set; }

        public List<Log> RecentLogs { get; set; } = new List<Log>();

        public Dictionary<string, int> MonthlyLossData { get; set; } = new Dictionary<string, int>();
    }
}
