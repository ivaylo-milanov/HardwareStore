namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Mouse;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MousesController : Controller
    {
        private readonly IProductService productService;
        private readonly IMemoryCache memoryCache;

        public MousesController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var mouses = await this.productService.GetProductsAsync<MouseViewModel>();
            this.memoryCache.Set("Mouses", mouses);

            return View(mouses);
        }

        public IActionResult FilterMouses([FromBody] MouseFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Mouses", out IEnumerable<MouseViewModel> mouses))
            {
                return BadRequest("Mouses data not found.");
            }

            IEnumerable<MouseViewModel> filtered = this.productService.FilterProducts(mouses, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
