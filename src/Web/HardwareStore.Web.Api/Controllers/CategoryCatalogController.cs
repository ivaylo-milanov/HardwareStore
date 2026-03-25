namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Web.Api.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/categories/{category}/catalog")]
    public class CategoryCatalogController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<CategoryCatalogController> logger;

        public CategoryCatalogController(IProductService productService, ILogger<CategoryCatalogController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CatalogPageViewModel>> Get(string category)
        {
            if (!await this.productService.CategoryExistsAsync(category))
            {
                this.logger.LogWarning(LogMessages.UnknownCategory, category);
                return this.NotFound();
            }

            try
            {
                var catalog = await this.productService.GetCategoryCatalogAsync(category);
                return this.Ok(new CatalogPageViewModel
                {
                    PageTitle = category,
                    CategoryKey = category,
                    Catalog = catalog,
                    SelectedOrder = 1,
                });
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, LogMessages.ProductRequestFailed);
                return this.Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("filter")]
        public async Task<ActionResult<CatalogPageViewModel>> Filter(string category, [FromBody] CatalogFilterRequest request)
        {
            if (!await this.productService.CategoryExistsAsync(category))
            {
                this.logger.LogWarning(LogMessages.UnknownCategory, category);
                return this.NotFound();
            }

            try
            {
                var baseCatalog = await this.productService.GetCategoryCatalogAsync(category);
                var filtered = await this.productService.FilterCategoryCatalogAsync(category, request.FilterJson);
                return this.Ok(new CatalogPageViewModel
                {
                    PageTitle = category,
                    CategoryKey = category,
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
                this.logger.LogError(ex, LogMessages.FilterFailedForCategory, category);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Could not apply filters.");
            }
        }
    }
}
