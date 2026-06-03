using MythosBalance.Models;

namespace MythosBalance.ViewModels
{
    public class GuideDetailViewModel
    {
        public MythologyGuide Guide { get; set; } = null!;
        public LifeDomain Domain { get; set; } = null!;
        public List<string> SymbolList { get; set; } = new();
    }

    public class AdminUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ActivityCount { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalActivities { get; set; }
        public int TotalGuides { get; set; }
        public List<AdminUserViewModel> RecentUsers { get; set; } = new();
    }
}
