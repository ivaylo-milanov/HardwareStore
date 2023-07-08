namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Ram;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class RamController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public RamController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var ram = await this.productService.GetProductsAsync<RamViewModel>();
            this.memoryCache.Set("Ram", ram);

            return View(ram);
        }

        public IActionResult FilterRam([FromBody] RamFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Ram", out IEnumerable<RamViewModel> ram))
            {
                return BadRequest("Ram data not found.");
            }

            IEnumerable<RamViewModel> filtered = this.productService.FilterProducts(ram, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
