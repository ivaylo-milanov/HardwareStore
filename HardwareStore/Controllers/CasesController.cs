namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Case;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class CasesController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public CasesController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var cases = await this.productService.GetProductsAsync<CaseViewModel>();
            this.memoryCache.Set("Cases", cases);

            return View(cases);
        }

        public IActionResult FilterCases([FromBody] CaseFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Cases", out IEnumerable<CaseViewModel> cases))
            {
                return BadRequest("Cases data not found.");
            }

            IEnumerable<CaseViewModel> filtered = this.productService.FilterProducts(cases, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
