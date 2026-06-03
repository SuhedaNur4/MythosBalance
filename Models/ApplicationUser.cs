using Microsoft.AspNetCore.Identity;

namespace MythosBalance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
