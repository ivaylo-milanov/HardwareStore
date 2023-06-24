namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Mouse;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MousesController : Controller
    {
        private readonly IMouseService mouseService;
        private readonly IMemoryCache memoryCache;

        public MousesController(IMouseService mouseService, IMemoryCache memoryCache)
        {
            this.mouseService = mouseService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            if (!this.memoryCache.TryGetValue("Mouses", out IEnumerable<MouseViewModel> mouses))
            {
                mouses = await this.mouseService.GetAllMouses();
                this.memoryCache.Set("Mouses", mouses);
            }

            return View(mouses);
        }

        [HttpPost]
        public IActionResult FilterMouses([FromBody] MouseFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Mouses", out IEnumerable<MouseViewModel> mouses))
            {
                return BadRequest("Mouses data not found.");
            }

            IEnumerable<MouseViewModel> filtered = this.mouseService.GetFilteredMouses(mouses, filter);
            return PartialView("_MousesPartialView", filtered);
        }
    }
}
