using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MythosBalance.Models;
using MythosBalance.Services;
using MythosBalance.ViewModels;

namespace MythosBalance.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivityController(IActivityService activityService, UserManager<ApplicationUser> userManager)
        {
            _activityService = activityService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? domainId, DateTime? startDate, DateTime? endDate)
        {
            var userId = _userManager.GetUserId(User)!;
            var vm = await _activityService.GetIndexViewModelAsync(userId, domainId, startDate, endDate);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = await _activityService.GetCreateViewModelAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var freshVm = await _activityService.GetCreateViewModelAsync();
                model.LifeDomainOptions = freshVm.LifeDomainOptions;
                return View(model);
            }

            var userId = _userManager.GetUserId(User)!;
            await _activityService.CreateAsync(model, userId);
            TempData["Success"] = "Aktivite başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var vm = await _activityService.GetEditViewModelAsync(id, userId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActivityEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var freshVm = await _activityService.GetCreateViewModelAsync();
                model.LifeDomainOptions = freshVm.LifeDomainOptions;
                return View(model);
            }

            var userId = _userManager.GetUserId(User)!;
            var success = await _activityService.UpdateAsync(model, userId);
            if (!success) return NotFound();

            TempData["Success"] = "Aktivite başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var vm = await _activityService.GetEditViewModelAsync(id, userId);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            await _activityService.DeleteAsync(id, userId);
            TempData["Success"] = "Aktivite silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
