namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Motherboard;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MotherboardsController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public MotherboardsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var motherboards = await this.productService.GetProductsAsync<MotherboardViewModel>();
            this.memoryCache.Set("Motherboards", motherboards);

            return View(motherboards);
        }

        public IActionResult FilterMotherboards([FromBody] MotherboardFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Motherboards", out IEnumerable<MotherboardViewModel> motherboards))
            {
                return BadRequest("Motherboards data not found.");
            }

            IEnumerable<MotherboardViewModel> filtered = this.productService.FilterProducts(motherboards, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
