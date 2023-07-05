namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using HardwareStore.Core.ViewModels.MousePad;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MousePadsController : Controller
    {
        private readonly IMousePadService mousePadService;
        private readonly IFilterService filterService;
        private readonly IMemoryCache memoryCache;

        public MousePadsController(
            IMousePadService mousePadService,
            IFilterService filterService,
            IMemoryCache memoryCache)
        {
            this.mousePadService = mousePadService;
            this.filterService = filterService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var mousePads = await this.mousePadService.GetAllProducts();
            this.memoryCache.Set("MousePads", mousePads);

            return View(mousePads);
        }

        public IActionResult FilterMousePads([FromBody] MousePadFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("MousePads", out IEnumerable<MousePadViewModel> mousePads))
            {
                return BadRequest("Mouse pads data not found.");
            }

            IEnumerable<MousePadViewModel> filtered = this.filterService.FilterProducts(mousePads, filter);

            return ViewComponent("ProductsComponent", filtered);
        }
    }
}
