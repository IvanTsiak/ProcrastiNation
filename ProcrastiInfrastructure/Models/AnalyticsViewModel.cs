namespace ProcrastiInfrastructure.Models
{
    public class AnalyticsViewModel
    {
        public int TotalUsers { get; set; }
        public int NewUsersToday { get; set; }
        public int TotalLogs { get; set; }
        public int LogsToday { get; set; }
        public int TotalWastedMinutes { get; set; }
        public int LossCount { get; set; }
        public int GainCount { get; set; }
    }
}
