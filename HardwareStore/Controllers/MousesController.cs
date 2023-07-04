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
        private readonly IFilterService filterService;

        public MousesController(IMouseService mouseService, IMemoryCache memoryCache, IFilterService filterService)
        {
            this.mouseService = mouseService;
            this.memoryCache = memoryCache;
            this.filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            var mouses = await this.mouseService.GetAllProducts();
            this.memoryCache.Set("Mouses", mouses);

            return View(mouses);
        }

        public IActionResult FilterMouses([FromBody] MouseFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Mouses", out IEnumerable<MouseViewModel> mouses))
            {
                return BadRequest("Mouses data not found.");
            }

            IEnumerable<MouseViewModel> filtered;

            filtered = this.filterService.FilterProducts(mouses, filter);
            filtered = this.filterService.OrderProducts(filtered, filter.Order);

            return ViewComponent("ProductsComponent", filtered);
        }
    }
}
