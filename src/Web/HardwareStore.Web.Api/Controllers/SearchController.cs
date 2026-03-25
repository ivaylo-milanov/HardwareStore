namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Web.Api.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<SearchController> logger;

        public SearchController(IProductService productService, ILogger<SearchController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CatalogPageViewModel>> Search([FromQuery] string? keyword)
        {
            var kw = keyword ?? string.Empty;
            try
            {
                var model = await this.productService.GetSearchCatalogAsync(kw);
                return this.Ok(new CatalogPageViewModel
                {
                    PageTitle = "Search",
                    SearchKeyword = kw,
                    Catalog = model,
                    SelectedOrder = 1,
                });
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, LogMessages.SearchCatalogFailed);
                return this.Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("filter")]
        public async Task<ActionResult<CatalogPageViewModel>> FilterSearch([FromBody] SearchFilterRequest request)
        {
            var keyword = request.Keyword ?? string.Empty;
            try
            {
                var baseCatalog = await this.productService.GetSearchCatalogAsync(keyword);
                var filtered = await this.productService.FilterSearchCatalogAsync(keyword, request.FilterJson);
                return this.Ok(new CatalogPageViewModel
                {
                    PageTitle = "Search",
                    SearchKeyword = keyword,
                    Catalog = new ProductsViewModel<CatalogProductViewModel>
                    {
                        Filters = baseCatalog.Filters,
                        Products = filtered.ToList(),
                    },
                    SelectedOrder = request.Order,
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, LogMessages.SearchFilterFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Could not apply filters.");
            }
        }
    }
}
