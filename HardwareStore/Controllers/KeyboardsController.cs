namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Keyboard;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class KeyboardsController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public KeyboardsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var keyboards = await this.productService.GetProductsAsync<KeyboardViewModel>();
            this.memoryCache.Set("Keyboards", keyboards);

            return View(keyboards);
        }

        public IActionResult FilterKeyboards([FromBody] KeyboardFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Keyboards", out IEnumerable<KeyboardViewModel> keyboards))
            {
                return BadRequest("Keyboards data not found.");
            }

            IEnumerable<KeyboardViewModel> filtered = this.productService.FilterProducts(keyboards, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
