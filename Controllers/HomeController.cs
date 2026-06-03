using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MythosBalance.Models;
using MythosBalance.Repositories;
using MythosBalance.Services;
using MythosBalance.ViewModels;

namespace MythosBalance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IActivityRepository _activityRepo;
        private readonly IGuideRepository _guideRepo;
        private readonly ILifeDomainRepository _domainRepo;
        private readonly INotificationService _notificationService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IActivityRepository activityRepo,
            IGuideRepository guideRepo,
            ILifeDomainRepository domainRepo,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _activityRepo = activityRepo;
            _guideRepo = guideRepo;
            _domainRepo = domainRepo;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            await _notificationService.GenerateRemindersIfNeededAsync(user.Id);

            var recentActivities = await _activityRepo.GetByUserIdAsync(user.Id);
            var guides = await _guideRepo.GetAllAsync();
            var domains = await _domainRepo.GetAllAsync();
            var countsByDomain = await _activityRepo.GetCountsByDomainAsync(user.Id);
            var countsByDomainThisMonth = await _activityRepo.GetCountsByDomainThisMonthAsync(user.Id);
            var pendingNotifications = await _notificationService.GetPendingNotificationsAsync(user.Id);

            var activityCountByDomain = new Dictionary<string, int>();
            var activityCountByDomainThisMonth = new Dictionary<string, int>();

            foreach (var domain in domains)
            {
                int adet = 0;
                if (countsByDomain.ContainsKey(domain.Id))
                    adet = countsByDomain[domain.Id];
                activityCountByDomain[domain.TurkishName] = adet;

                int adetThisMonth = 0;
                if (countsByDomainThisMonth.ContainsKey(domain.Id))
                    adetThisMonth = countsByDomainThisMonth[domain.Id];
                activityCountByDomainThisMonth[domain.TurkishName] = adetThisMonth;
            }

            string? mostActiveDomain = null;
            int enYuksekSayi = 0;
            int enAktifDomainId = 0;
            foreach (var kvp in countsByDomain)
            {
                if (kvp.Value > enYuksekSayi)
                {
                    enYuksekSayi = kvp.Value;
                    enAktifDomainId = kvp.Key;
                }
            }
            if (enYuksekSayi > 0)
            {
                foreach (var d in domains)
                {
                    if (d.Id == enAktifDomainId)
                    {
                        mostActiveDomain = d.TurkishName;
                        break;
                    }
                }
            }

            var vm = new DashboardViewModel
            {
                DisplayName = user.DisplayName,
                RecentActivities = recentActivities.Take(5).ToList(),
                LifeDomains = domains,
                Guides = guides,
                ActivityCountByDomain = activityCountByDomain,
                PendingNotifications = pendingNotifications,
                WeeklyActivityCount = await _activityRepo.CountThisWeekByUserAsync(user.Id),
                TotalActivityCount = await _activityRepo.CountByUserAsync(user.Id),
                MostActiveDomainName = mostActiveDomain,
                ActivityCountByDomainThisMonth = activityCountByDomainThisMonth,
                TotalActivityCountThisMonth = await _activityRepo.CountThisMonthByUserAsync(user.Id),
                CurrentMonthName = DateTime.Today.ToString("MMMM", new System.Globalization.CultureInfo("tr-TR"))
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DismissNotification(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DismissAllNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                await _notificationService.MarkAllAsReadAsync(user.Id);
            return Ok();
        }
    }
}
