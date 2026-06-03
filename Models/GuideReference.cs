using System.ComponentModel.DataAnnotations;

namespace MythosBalance.Models
{
    public class GuideReference
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Url { get; set; }

        [MaxLength(200)]
        public string? Author { get; set; }

        public int? Year { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }
        public int MythologyGuideId { get; set; }
        public MythologyGuide MythologyGuide { get; set; } = null!;
    }
}
