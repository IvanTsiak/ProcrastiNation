using System.Collections.Generic;
using ProcrastiDomain.Model;

namespace ProcrastiInfrastructure.Models
{
    public class GlobalStatsViewModel
    {
        public List<Log> TopComments { get; set; }
        public List<User> TopUsers { get; set; }
        public List<Activity> TopActivities { get; set; }
    }
}
