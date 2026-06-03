using System.ComponentModel.DataAnnotations;

namespace MythosBalance.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int? LifeDomainId { get; set; }
        public LifeDomain? LifeDomain { get; set; }
    }
}
