namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
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
            ProductsViewModel<CatalogProductViewModel> model;
            try
            {
                model = await this.productService.GetSearchCatalogAsync(keyword ?? string.Empty).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            var page = new CatalogPageViewModel
            {
                PageTitle = "Search",
                SearchKeyword = keyword ?? string.Empty,
                Catalog = model,
            };
            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> FilterSearchedProducts([FromBody] System.Text.Json.JsonElement body, string keyword)
        {
            try
            {
                var filtered = await this.productService
                    .FilterSearchCatalogAsync(keyword ?? string.Empty, body.GetRawText())
                    .ConfigureAwait(false);
                return PartialView("_ProductsPartialView", filtered);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Search filter failed");
                return PartialView("_ProductsPartialView", Enumerable.Empty<CatalogProductViewModel>());
            }
        }
    }
}
