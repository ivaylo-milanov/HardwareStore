namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Extensions;
    using HardwareStore.Web.Mvc.Helpers;
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
                    SelectedOrder = 1,
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
        public async Task<IActionResult> Filter(string category)
        {
            if (!await this.productService.CategoryExistsAsync(category).ConfigureAwait(false))
            {
                this.logger.LogWarning("Unknown category: {Category}", category);
                return this.RedirectToAction("Error", "Home", new { message = "Invalid category." });
            }

            var filterJson = CatalogFilterFormHelper.BuildFilterJson(this.Request.Form);
            var selected = CatalogFilterFormHelper.ParseSelectedFilters(this.Request.Form);
            var orderStr = this.Request.Form["Order"].FirstOrDefault();
            var selectedOrder = 1;
            if (!string.IsNullOrEmpty(orderStr) && int.TryParse(orderStr, out var o))
            {
                selectedOrder = o;
            }

            try
            {
                var baseCatalog = await this.productService.GetCategoryCatalogAsync(category).ConfigureAwait(false);
                var filtered = await this.productService
                    .FilterCategoryCatalogAsync(category, filterJson)
                    .ConfigureAwait(false);
                var page = new CatalogPageViewModel
                {
                    PageTitle = this.Request.Form["pageTitle"].FirstOrDefault() ?? category,
                    CategoryKey = category,
                    Catalog = new ProductsViewModel<CatalogProductViewModel>
                    {
                        Filters = baseCatalog.Filters,
                        Products = filtered.ToList(),
                    },
                    SelectedOrder = selectedOrder,
                    SelectedFilterValues = selected,
                };
                return this.View("Catalog", page);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Filter failed for category {Category}", category);
                return this.RedirectToAction("Error", "Home", new { message = "Could not apply filters." });
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
            if (!(this.User?.Identity?.IsAuthenticated ?? false))
            {
                return false;
            }

            try
            {
                return await this.productService
                    .IsProductInDbFavorites(this.User.GetUserId(), productId)
                    .ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return false;
            }
        }

    }
}
