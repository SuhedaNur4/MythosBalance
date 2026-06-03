using Microsoft.EntityFrameworkCore;
using MythosBalance.Models;
using MythosBalance.Repositories;

namespace MythosBalance.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetPendingNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
        Task GenerateRemindersIfNeededAsync(string userId);
    }

    public class NotificationService : INotificationService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly ILifeDomainRepository _domainRepo;
        private readonly Data.ApplicationDbContext _context;
        private const int ReminderThresholdDays = 7;

        private static readonly Dictionary<string, string> DomainReminders = new Dictionary<string, string>
        {
            { "Health", "Kendinize biraz zaman ayırmak iyi gelebilir. Küçük bir yürüyüş bile fark yaratır." },
            { "Education", "Yeni şeyler öğrenmek için küçük bir adım atabilirsiniz. Bir kitap sayfası bile yeterli." },
            { "Creativity", "Yaratıcı düşünceniz sizi bekliyor. Bugün bir şeyler yaratmak nasıl hissettirirdi?" },
            { "Travel", "Yeni bir yer keşfetmek bazen bakış açımızı tamamen değiştirebilir." }
        };

        private static readonly string[] SocialReminders = new string[]
        {
            "Uzun zamandır bir dostunuza destek olduğunuz bir anı kaydetmediniz. Küçük bir nezaket büyük anlamlar taşıyabilir.",
            "Son günlerde keyifli sosyal anılar eklenmemiş görünüyor. Güzel anlar paylaşıldıkça çoğalır.",
            "Yeni etkinliklere veya topluluklara katılmak farklı deneyimler kazandırabilir."
        };

        public NotificationService(
            IActivityRepository activityRepo,
            ILifeDomainRepository domainRepo,
            Data.ApplicationDbContext context)
        {
            _activityRepo = activityRepo;
            _domainRepo = domainRepo;
            _context = context;
        }

        public async Task<List<Notification>> GetPendingNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .Include(n => n.LifeDomain)
                    .ThenInclude(ld => ld!.MythologyGuide)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
            foreach (var notif in notifications)
            {
                notif.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task GenerateRemindersIfNeededAsync(string userId)
        {
            var domains = await _domainRepo.GetAllAsync();
            var threshold = DateTime.Today.AddDays(-ReminderThresholdDays);

            foreach (var domain in domains)
            {
                var lastDate = await _activityRepo.GetLastActivityDateByDomainAsync(userId, domain.Id);
                if (lastDate == null || lastDate.Value.Date < threshold)
                {
                    bool zatenVar = await _context.Notifications
                        .AnyAsync(n => n.UserId == userId && n.LifeDomainId == domain.Id && n.CreatedAt >= threshold);

                    if (!zatenVar)
                    {
                        string? message = null;
                        if (domain.Name == "Social")
                        {
                            var rng = new Random();
                            message = SocialReminders[rng.Next(SocialReminders.Length)];
                        }
                        else if (DomainReminders.ContainsKey(domain.Name))
                        {
                            message = DomainReminders[domain.Name];
                        }

                        if (!string.IsNullOrEmpty(message))
                        {
                            _context.Notifications.Add(new Notification
                            {
                                UserId = userId,
                                LifeDomainId = domain.Id,
                                Message = message,
                                IsRead = false,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
