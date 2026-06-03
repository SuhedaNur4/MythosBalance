using System.ComponentModel.DataAnnotations;

namespace MythosBalance.Models
{
    public class LifeDomain
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string TurkishName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        public string IconClass { get; set; } = "bi bi-circle";

        [MaxLength(20)]
        public string ColorHex { get; set; } = "#c9a84c";

        public MythologyGuide? MythologyGuide { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
