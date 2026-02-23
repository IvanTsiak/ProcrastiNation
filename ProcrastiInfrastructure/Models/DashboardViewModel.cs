using System.Collections.Generic;
using ProcrastiDomain.Model;

namespace ProcrastiInfrastructure.Models
{
    public class DashboardViewModel
    {
        public int GlobalLossAmount { get; set; }
        public List<Log> RecentLogs { get; set; }
    }
}
