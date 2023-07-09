namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.PowerSupply;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class PowerSuppliesController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public PowerSuppliesController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var powerSupplies = await this.productService.GetProductsAsync<PowerSupplyViewModel>();
            this.memoryCache.Set("PowerSupplies", powerSupplies);

            return View(powerSupplies);
        }

        public IActionResult FilterPowerSupplies([FromBody] PowerSupplyFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("PowerSupplies", out IEnumerable<PowerSupplyViewModel> powerSupplies))
            {
                return BadRequest("Power supplies data not found.");
            }

            IEnumerable<PowerSupplyViewModel> filtered = this.productService.FilterProducts(powerSupplies, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
