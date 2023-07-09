namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Cooler;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class CoolersController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public CoolersController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var coolers = await this.productService.GetProductsAsync<CoolerViewModel>();
            this.memoryCache.Set("Coolers", coolers);

            return View(coolers);
        }

        public IActionResult FilterCoolers([FromBody] CoolerFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Coolers", out IEnumerable<CoolerViewModel> coolers))
            {
                return BadRequest("Coolers data not found.");
            }

            IEnumerable<CoolerViewModel> filtered = this.productService.FilterProducts(coolers, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
