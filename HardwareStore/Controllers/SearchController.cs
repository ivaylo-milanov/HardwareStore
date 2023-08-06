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
                model = await this.productService.GetSearchModel(keyword);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterSearchedProducts([FromBody] SearchFilterOptions filter)
        {
            IEnumerable<SearchViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<SearchViewModel, SearchFilterOptions>(filter);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
