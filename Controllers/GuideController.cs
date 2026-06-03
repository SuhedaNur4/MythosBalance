using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MythosBalance.Repositories;
using MythosBalance.ViewModels;

namespace MythosBalance.Controllers
{
    [Authorize]
    public class GuideController : Controller
    {
        private readonly IGuideRepository _guideRepo;

        public GuideController(IGuideRepository guideRepo)
        {
            _guideRepo = guideRepo;
        }

        public async Task<IActionResult> Index()
        {
            var guides = await _guideRepo.GetAllAsync();
            return View(guides);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var guide = await _guideRepo.GetByIdAsync(id);
            if (guide == null) return NotFound();
            var parcalar = guide.Symbols.Split(',');
            var symbolList = new List<string>();
            foreach (var parca in parcalar)
            {
                var temiz = parca.Trim();
                if (temiz.Length > 0)
                    symbolList.Add(temiz);
            }

            var vm = new GuideDetailViewModel
            {
                Guide = guide,
                Domain = guide.LifeDomain,
                SymbolList = symbolList
            };

            return View(vm);
        }
    }
}
