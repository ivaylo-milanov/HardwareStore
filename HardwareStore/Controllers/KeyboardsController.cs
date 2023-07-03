namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Keyboard;
    using HardwareStore.Core.ViewModels.Mouse;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class KeyboardsController : Controller
    {
        private readonly IKeyboardService keyboardService;
        private readonly IMemoryCache memoryCache;

        public KeyboardsController(IKeyboardService keyboardService, IMemoryCache memoryCache)
        {
            this.keyboardService = keyboardService;
            this.memoryCache = memoryCache;
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

            IEnumerable<KeyboardViewModel> filtered = keyboards;
            if (filter != null)
            {
                filtered = filtered.GetFilteredProducts(filter);
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
