namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class SearchController : Controller
    {
        private readonly IProductService productService;
        private readonly IMemoryCache memoryCache;

        public SearchController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var products = await this.productService.GetProductsByKeyword(keyword);
            this.memoryCache.Set("Products", products);

            return RedirectToAction("Index");
        }

        public IActionResult FilterSearchedProducts([FromBody] ProductFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Products", out IEnumerable<ProductViewModel> products))
            {
                return BadRequest("Searched data not found.");
            }

            IEnumerable<ProductViewModel> filtered = this.productService.FilterProducts(products, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
