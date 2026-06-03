using System.ComponentModel.DataAnnotations;

namespace MythosBalance.Models
{
    public class MythologyGuide
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(400)]
        public string ShortDescription { get; set; } = string.Empty;

        public string FullDescription { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Symbols { get; set; } = string.Empty;

        public string HistoricalBackground { get; set; } = string.Empty;

        public string WhyThisGuide { get; set; } = string.Empty;

        public string MythologicalStory { get; set; } = string.Empty;

        [MaxLength(200)]
        public string ImagePath { get; set; } = string.Empty;

        public int LifeDomainId { get; set; }
        public LifeDomain LifeDomain { get; set; } = null!;

        public ICollection<GuideReference> References { get; set; } = new List<GuideReference>();
    }
}
