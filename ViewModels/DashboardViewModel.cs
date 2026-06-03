using MythosBalance.Models;

namespace MythosBalance.ViewModels
{
    public class DashboardViewModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public List<Activity> RecentActivities { get; set; } = new();
        public List<LifeDomain> LifeDomains { get; set; } = new();
        public List<MythologyGuide> Guides { get; set; } = new();
        public Dictionary<string, int> ActivityCountByDomain { get; set; } = new();
        public List<Notification> PendingNotifications { get; set; } = new();
        public int WeeklyActivityCount { get; set; }

        public int TotalActivityCount { get; set; }

        public string? MostActiveDomainName { get; set; }

        public Dictionary<string, int> ActivityCountByDomainThisMonth { get; set; } = new();
        public int TotalActivityCountThisMonth { get; set; }
        public string CurrentMonthName { get; set; } = string.Empty;
    }
}
