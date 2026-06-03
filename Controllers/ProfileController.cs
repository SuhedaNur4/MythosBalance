using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MythosBalance.Models;
using MythosBalance.Repositories;
using MythosBalance.ViewModels;

namespace MythosBalance.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IActivityRepository _activityRepo;
        private readonly ILifeDomainRepository _domainRepo;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IActivityRepository activityRepo,
            ILifeDomainRepository domainRepo)
        {
            _userManager = userManager;
            _activityRepo = activityRepo;
            _domainRepo = domainRepo;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var allActivities = await _activityRepo.GetByUserIdAsync(user.Id);
            var domains = await _domainRepo.GetAllAsync();
            var countsByDomain = await _activityRepo.GetCountsByDomainAsync(user.Id);

            var activitiesByDomain = new List<DomainActivityStat>();
            foreach (var d in domains)
            {
                int count = 0;
                if (countsByDomain.ContainsKey(d.Id))
                    count = countsByDomain[d.Id];
                string imgPath = "";
                if (d.Name == "Health")
                    imgPath = "/images/gods/Hygieia2.jpg";
                else if (d.Name == "Education")
                    imgPath = "/images/gods/Athena2.jpg";
                else if (d.Name == "Creativity")
                    imgPath = "/images/gods/Apollo2.jpg";
                else if (d.Name == "Travel")
                    imgPath = "/images/gods/Hermes2.jpg";
                else if (d.Name == "Social")
                    imgPath = "/images/gods/Charites2.png";
                else if (d.MythologyGuide != null && d.MythologyGuide.ImagePath != null)
                    imgPath = d.MythologyGuide.ImagePath;

                activitiesByDomain.Add(new DomainActivityStat
                {
                    DomainId = d.Id,
                    DomainName = d.TurkishName,
                    Count = count,
                    ColorHex = d.ColorHex,
                    ImagePath = imgPath
                });
            }

            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var vm = new ProfileViewModel
            {
                Email = user.Email ?? string.Empty,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                CreatedAt = user.CreatedAt,
                TotalActivities = allActivities.Count,
                ActivitiesThisMonth = allActivities.Count(a => a.Date >= startOfMonth),
                ActivitiesByDomain = activitiesByDomain,
                RecentActivities = allActivities.Take(5).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var vm = new ProfileEditViewModel
            {
                DisplayName = user.DisplayName,
                Bio = user.Bio
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;

            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DomainDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var domain = await _domainRepo.GetByIdAsync(id);
            if (domain == null) return NotFound();

            var activities = await _activityRepo.GetByUserIdFilteredAsync(user.Id, id, null, null);

            var groupedActivities = new Dictionary<string, List<Activity>>();
            foreach (var act in activities.OrderByDescending(a => a.Date))
            {
                var monthKey = act.Date.ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"));
                if (!groupedActivities.ContainsKey(monthKey))
                {
                    groupedActivities[monthKey] = new List<Activity>();
                }
                groupedActivities[monthKey].Add(act);
            }

            string quote = domain.Name switch
            {
                "Hygieia" => "\"Bedenine iyi bak ki ruhun özgürleşsin.\"",
                "Athena" => "\"Bilgelik, dengeli bir yaşamın anahtarıdır.\"",
                "Apollo" => "\"Yaratıcılık, içindeki ışığın dışa vurumudur.\"",
                "Hermes" => "\"Her yeni adım, yeni bir keşiftir.\"",
                "Charites" => "\"Bağ kurmak, ruhu besleyen en tatlı meyvedir.\"",
                _ => "\"Denge, büyümenin temelidir.\""
            };

            var vm = new DomainDetailViewModel
            {
                Domain = domain,
                Quote = quote,
                ActivitiesByMonth = groupedActivities
            };

            return View(vm);
        }
    }
}
