namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductController> logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this.logger = logger;
            this.productService = productService;
        }

        public async Task<IActionResult> Index(string category, string title)
        {
            if (!await this.productService.CategoryExistsAsync(category).ConfigureAwait(false))
            {
                this.logger.LogWarning("Unknown category: {Category}", category);
                return RedirectToAction("Error", "Home", new { message = "Invalid category." });
            }

            try
            {
                var catalog = await this.productService.GetCategoryCatalogAsync(category).ConfigureAwait(false);
                var page = new CatalogPageViewModel
                {
                    PageTitle = title,
                    CategoryKey = category,
                    Catalog = catalog,
                    FilterPostUrl = this.Url.Action("Filter", "Product", new { category })!,
                };
                return this.View("Catalog", page);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] System.Text.Json.JsonElement body, string category)
        {
            if (!await this.productService.CategoryExistsAsync(category).ConfigureAwait(false))
            {
                this.logger.LogWarning("Unknown category: {Category}", category);
                return this.EmptyProductsPartial();
            }

            try
            {
                var filtered = await this.productService
                    .FilterCategoryCatalogAsync(category, body.GetRawText())
                    .ConfigureAwait(false);
                return PartialView("_ProductsPartialView", filtered);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Filter failed for category {Category}", category);
                return this.EmptyProductsPartial();
            }
        }

        public async Task<IActionResult> Details(int productId)
        {
            ProductDetailsModel model;
            try
            {
                model = await this.productService.GetProductDetails(productId).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            model.IsFavorite = await this.ResolveFavoriteStateAsync(productId).ConfigureAwait(false);

            return this.View(model);
        }

        private async Task<bool> ResolveFavoriteStateAsync(int productId)
        {
            try
            {
                var sessionFavorites = this.HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();
                if (this.User?.Identity?.IsAuthenticated ?? false)
                {
                    return await this.productService
                        .IsProductInDbFavorites(this.User.GetUserId(), productId)
                        .ConfigureAwait(false);
                }

                return await this.productService
                    .IsProductInSessionFavorites(sessionFavorites, productId)
                    .ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private PartialViewResult EmptyProductsPartial() =>
            PartialView("_ProductsPartialView", Enumerable.Empty<CatalogProductViewModel>());
    }
}
