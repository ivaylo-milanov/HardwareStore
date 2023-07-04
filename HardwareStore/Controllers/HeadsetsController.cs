namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class HeadsetsController : Controller
    {
        private readonly IHeadsetService headsetService;
        private readonly IMemoryCache memoryCache;
        private readonly IFilterService filterService;

        public HeadsetsController(
            IHeadsetService headsetService,
            IMemoryCache memoryCache,
            IFilterService filterService)
        {
            this.headsetService = headsetService;
            this.memoryCache = memoryCache;
            this.filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            var headsets = await this.headsetService.GetAllProducts();
            this.memoryCache.Set("Headsets", headsets);

            return View(headsets);
        }

        public IActionResult FilterHeadsets([FromBody] HeadsetFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Headsets", out IEnumerable<HeadsetViewModel> headsets))
            {
                return BadRequest("Headsets data not found.");
            }

            IEnumerable<HeadsetViewModel> filtered;

            filtered = this.filterService.FilterProducts(headsets, filter);
            filtered = this.filterService.OrderProducts(filtered, filter.Order);

            return ViewComponent("ProductComponent", filtered);
        }
    }
}