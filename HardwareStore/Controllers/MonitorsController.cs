namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Monitor;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MonitorsController : Controller
    {
        private readonly IMonitorService monitorService;
        private readonly IMemoryCache memoryCache;
        private readonly IFilterService filterService;

        public MonitorsController(
            IMonitorService monitorService,
            IMemoryCache memoryCache,
            IFilterService filterService)
        {
            this.monitorService = monitorService;
            this.memoryCache = memoryCache;
            this.filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            var monitors = await this.monitorService.GetAllProducts();
            this.memoryCache.Set("Monitors", monitors);

            return View(monitors);
        }

        public IActionResult FilterMonitors([FromBody] MonitorFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Monitors", out IEnumerable<MonitorViewModel> monitors))
            {
                return BadRequest("Monitors data not found.");
            }

            IEnumerable<MonitorViewModel> filtered;

            filtered = this.filterService.FilterProducts(monitors, filter);
            filtered = this.filterService.OrderProducts(filtered, filter.Order);

            return ViewComponent("ProductsComponent", filtered);
        }
    }
}
