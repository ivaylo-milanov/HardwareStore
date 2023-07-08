namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Monitor;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MonitorsController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public MonitorsController(IMemoryCache memoryCache, IProductService productService)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var monitors = await this.productService.GetProductsAsync<MonitorViewModel>();
            this.memoryCache.Set("Monitors", monitors);

            return View(monitors);
        }

        public IActionResult FilterMonitors([FromBody] MonitorFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Monitors", out IEnumerable<MonitorViewModel> monitors))
            {
                return BadRequest("Monitors data not found.");
            }

            IEnumerable<MonitorViewModel> filtered = this.productService.FilterProducts(monitors, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}