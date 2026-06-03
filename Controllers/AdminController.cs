using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MythosBalance.Data;
using MythosBalance.Models;
using MythosBalance.Repositories;
using MythosBalance.ViewModels;

namespace MythosBalance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IGuideRepository _guideRepo;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IGuideRepository guideRepo)
        {
            _userManager = userManager;
            _context = context;
            _guideRepo = guideRepo;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var siraliUsers = await _userManager.Users.OrderByDescending(x => x.CreatedAt).Take(5).ToListAsync();
            var recentUsers = new List<AdminUserViewModel>();

            foreach (var u in siraliUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                string rol = "User";
                if (roles.Count > 0)
                {
                    rol = roles[0];
                }

                recentUsers.Add(new AdminUserViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Email ?? "",
                    Role = rol,
                    CreatedAt = u.CreatedAt,
                    ActivityCount = await _context.Activities.CountAsync(a => a.UserId == u.Id)
                });
            }

            var vm = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalActivities = await _context.Activities.CountAsync(),
                TotalGuides = await _context.MythologyGuides.CountAsync(),
                RecentUsers = recentUsers
            };

            return View(vm);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            var viewModels = new List<AdminUserViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                string rol = "User";
                if (roles.Count > 0)
                {
                    rol = roles[0];
                }

                viewModels.Add(new AdminUserViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Email ?? "",
                    Role = rol,
                    CreatedAt = u.CreatedAt,
                    ActivityCount = await _context.Activities.CountAsync(a => a.UserId == u.Id)
                });
            }

            return View(viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                await _userManager.AddToRoleAsync(user, "User");
                TempData["Success"] = $"{user.DisplayName} artık normal kullanıcı.";
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["Success"] = $"{user.DisplayName} artık yönetici.";
            }

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> Guides()
        {
            var guides = await _guideRepo.GetAllAsync();
            return View(guides);
        }
    }
}
