using System.ComponentModel.DataAnnotations;
using MythosBalance.Models;

namespace MythosBalance.ViewModels
{
    public class DomainActivityStat
    {
        public int DomainId { get; set; }
        public string DomainName { get; set; } = string.Empty;
        public int Count { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
    }

    public class ProfileViewModel
    {
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Görünen ad zorunludur.")]
        [MaxLength(100)]
        [Display(Name = "Görünen Ad")]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(300)]
        [Display(Name = "Hakkımda")]
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; }
        public int TotalActivities { get; set; }
        public int ActivitiesThisMonth { get; set; }
        public List<DomainActivityStat> ActivitiesByDomain { get; set; } = new();
        public List<Activity> RecentActivities { get; set; } = new();
    }

    public class ProfileEditViewModel
    {
        [Required(ErrorMessage = "Görünen ad zorunludur.")]
        [MaxLength(100)]
        [Display(Name = "Görünen Ad")]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(300)]
        [Display(Name = "Hakkımda")]
        public string? Bio { get; set; }
    }

    public class DomainDetailViewModel
    {
        public LifeDomain Domain { get; set; } = null!;
        public string Quote { get; set; } = string.Empty;
        public Dictionary<string, List<Activity>> ActivitiesByMonth { get; set; } = new();
    }
}
