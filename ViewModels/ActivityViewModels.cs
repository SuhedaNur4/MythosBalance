using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MythosBalance.Models;

namespace MythosBalance.ViewModels
{
    public class ActivityCreateViewModel
    {
        [Required(ErrorMessage = "Aktivite başlığı zorunludur.")]
        [MaxLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        [Display(Name = "Aktivite Başlığı")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Range(1, 1440, ErrorMessage = "Süre 1 ile 1440 dakika arasında olmalıdır.")]
        [Display(Name = "Süre (Dakika)")]
        public int? DurationMinutes { get; set; }

        [Required(ErrorMessage = "Lütfen bir yaşam alanı seçin.")]
        [Display(Name = "Yaşam Alanı")]
        public int LifeDomainId { get; set; }

        public List<SelectListItem> LifeDomainOptions { get; set; } = new();
    }

    public class ActivityEditViewModel : ActivityCreateViewModel
    {
        public int Id { get; set; }
    }

    public class ActivityIndexViewModel
    {
        public List<Activity> Activities { get; set; } = new();
        public List<LifeDomain> LifeDomains { get; set; } = new();
        public int? FilterDomainId { get; set; }
        public string? FilterDomainName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
