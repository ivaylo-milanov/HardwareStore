namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Keyboard;
    using HardwareStore.Core.ViewModels.Mouse;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class KeyboardsController : Controller
    {
        private readonly IKeyboardService keyboardService;
        private readonly IMemoryCache memoryCache;
        private readonly IFilterService filterService;

        public KeyboardsController(IKeyboardService keyboardService, IMemoryCache memoryCache, IFilterService filterService)
        {
            this.keyboardService = keyboardService;
            this.memoryCache = memoryCache;
            this.filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            var keyboards = await this.keyboardService.GetAllProducts();
            this.memoryCache.Set("Keyboards", keyboards);

            return View(keyboards);
        }

        public IActionResult FilterKeyboards([FromBody] KeyboardFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Keyboards", out IEnumerable<KeyboardViewModel> keyboards))
            {
                return BadRequest("Keyboards data not found.");
            }

            IEnumerable<KeyboardViewModel> filtered;

            filtered = this.filterService.FilterProducts(keyboards, filter);
            filtered = this.filterService.OrderProducts(filtered, filter.Order);

            return ViewComponent("ProductsComponent", filtered);
        }
    }
}
