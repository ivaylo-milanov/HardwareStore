namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Web.Mvc.Helpers;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : Controller
    {
        #region Fields and construction

        private readonly IProductService productService;
        private readonly ILogger<SearchController> logger;

        public SearchController(IProductService productService, ILogger<SearchController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        #endregion

        #region Search catalog

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> Index(string keyword)
        {
            ProductsViewModel<CatalogProductViewModel> model;
            try
            {
                model = await this.productService.GetSearchCatalogAsync(keyword ?? string.Empty).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, LogMessages.SearchCatalogFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            var kw = keyword ?? string.Empty;
            var page = new CatalogPageViewModel
            {
                PageTitle = "Search",
                SearchKeyword = kw,
                Catalog = model,
                SelectedOrder = 1,
            };
            return this.View("~/Views/Product/Catalog.cshtml", page);
        }

        #endregion

        #region Search filter (POST)

        [HttpPost]
        public async Task<IActionResult> FilterSearch()
        {
            var keyword = this.Request.Form["keyword"].FirstOrDefault() ?? string.Empty;
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
                var baseCatalog = await this.productService.GetSearchCatalogAsync(keyword).ConfigureAwait(false);
                var filtered = await this.productService
                    .FilterSearchCatalogAsync(keyword, filterJson)
                    .ConfigureAwait(false);
                var page = new CatalogPageViewModel
                {
                    PageTitle = this.Request.Form["pageTitle"].FirstOrDefault() ?? "Search",
                    SearchKeyword = keyword,
                    Catalog = new ProductsViewModel<CatalogProductViewModel>
                    {
                        Filters = baseCatalog.Filters,
                        Products = filtered.ToList(),
                    },
                    SelectedOrder = selectedOrder,
                    SelectedFilterValues = selected,
                };
                return this.View("~/Views/Product/Catalog.cshtml", page);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, LogMessages.SearchFilterFailed);
                return this.RedirectToAction("Error", "Home", new { message = "Could not apply filters." });
            }
        }

        #endregion
    }
}
