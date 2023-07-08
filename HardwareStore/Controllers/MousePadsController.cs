namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.MousePad;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class MousePadsController : Controller
    {
        private readonly IProductService productService;
        private readonly IMemoryCache memoryCache;

        public MousePadsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var mousePads = await this.productService.GetProductsAsync<MousePadViewModel>();
            this.memoryCache.Set("MousePads", mousePads);

            return View(mousePads);
        }

        public IActionResult FilterMousePads([FromBody] MousePadFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("MousePads", out IEnumerable<MousePadViewModel> mousePads))
            {
                return BadRequest("Mouse pads data not found.");
            }

            IEnumerable<MousePadViewModel> filtered = this.productService.FilterProducts(mousePads, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
