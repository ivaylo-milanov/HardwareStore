namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<SearchController> logger;

        public SearchController(IProductService productService, ILogger<SearchController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keyword)
        {
            ProductsViewModel<SearchViewModel> model;
            try
            {
                model = await this.productService.GetModel<SearchViewModel>(keyword);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            ViewBag.SearchKeyword = keyword ?? string.Empty;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FilterSearchedProducts([FromBody] SearchFilterOptions filter, string keyword)
        {
            try
            {
                var filtered = await this.productService.FilterSearchProductsAsync(keyword ?? string.Empty, filter).ConfigureAwait(false);
                return PartialView("_ProductsPartialView", filtered);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Search filter failed");
                return PartialView("_ProductsPartialView", Enumerable.Empty<SearchViewModel>());
            }
        }
    }
}
