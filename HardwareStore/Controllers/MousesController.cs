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
            MousesViewModel model = await this.mouseService.GetModel();
            this.memoryCache.Set("Mouses", model.Mouses);

            return View(model);
        }

        [HttpPost]
        public IActionResult FilterMouses([FromBody] MouseFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Mouses", out IEnumerable<MouseViewModel> mouses))
            {
                return BadRequest("Mouses data not found.");
            }

            IEnumerable<MouseViewModel> filtered = mouses;
            if (filter != null)
            {
                filtered = this.mouseService.GetFilteredMouses(mouses, filter);
            }

            return PartialView("_MousesPartialView", filtered);
        }
    }
}
