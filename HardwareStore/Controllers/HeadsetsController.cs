namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class HeadsetsController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public HeadsetsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var headsets = await this.productService.GetProductsAsync<HeadsetViewModel>();
            this.memoryCache.Set("Headsets", headsets);

            return View(headsets);
        }

        public IActionResult FilterHeadsets([FromBody] HeadsetFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Headsets", out IEnumerable<HeadsetViewModel> headsets))
            {
                return BadRequest("Headsets data not found.");
            }

            IEnumerable<HeadsetViewModel> filtered = this.productService.FilterProducts(headsets, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}