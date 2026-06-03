using System.ComponentModel.DataAnnotations;

namespace MythosBalance.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Aktivite başlığı zorunludur.")]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Range(1, 1440, ErrorMessage = "Süre 1 ile 1440 dakika arasında olmalıdır.")]
        public int? DurationMinutes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen bir yaşam alanı seçin.")]
        public int LifeDomainId { get; set; }
        public LifeDomain LifeDomain { get; set; } = null!;
    }
}
