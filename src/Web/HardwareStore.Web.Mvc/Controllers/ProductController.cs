namespace HardwareStore.Web.Mvc.Controllers
{
    using System.Text.Json;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private static readonly JsonSerializerOptions FilterJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        private readonly IDetailsService detailsService;
        private readonly IProductService productService;
        private readonly ILogger<ProductController> logger;

        public ProductController(IDetailsService detailsService, ILogger<ProductController> logger, IProductService productService)
        {
            this.logger = logger;
            this.detailsService = detailsService;
            this.productService = productService;
        }

        public async Task<IActionResult> Index(string category, string title)
        {
            if (!ProductCategoryTypes.TryResolve(category, out var viewModelType, out _))
            {
                this.logger.LogWarning("Unknown category: {Category}", category);
                return RedirectToAction("Error", "Home", new { message = "Invalid category." });
            }

            try
            {
                ViewBag.Model = await this.InvokeGetModelAsync(viewModelType);
                ViewBag.Title = title;
                ViewBag.Category = category;
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] JsonElement body, string category)
        {
            if (!ProductCategoryTypes.TryResolve(category, out var viewModelType, out var filterOptionsType))
            {
                this.logger.LogWarning("Unknown category: {Category}", category);
                return this.EmptyProductsPartial();
            }

            try
            {
                var filterDto = JsonSerializer.Deserialize(body.GetRawText(), filterOptionsType, FilterJsonOptions);
                if (filterDto == null)
                    return this.EmptyProductsPartial();

                var filtered = await this.InvokeFilterProductsAsync(viewModelType, filterOptionsType, filterDto).ConfigureAwait(false);
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
                model = await this.detailsService.GetProductDetails(productId);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        private async Task<object> InvokeGetModelAsync(Type viewModelType)
        {
            var method = typeof(IProductService).GetMethod(nameof(IProductService.GetModel))!
                .MakeGenericMethod(viewModelType);
            var task = (Task)method.Invoke(this.productService, new object?[] { null })!;
            await task.ConfigureAwait(false);
            return task.GetType().GetProperty("Result")!.GetValue(task)!;
        }

        private async Task<object> InvokeFilterProductsAsync(Type viewModelType, Type filterOptionsType, object filterDto)
        {
            var method = typeof(IProductService).GetMethod(nameof(IProductService.FilterProductsAsync))!
                .MakeGenericMethod(viewModelType, filterOptionsType);
            var task = (Task)method.Invoke(this.productService, new[] { filterDto })!;
            await task.ConfigureAwait(false);
            return task.GetType().GetProperty("Result")!.GetValue(task)!;
        }

        private PartialViewResult EmptyProductsPartial() =>
            PartialView("_ProductsPartialView", Enumerable.Empty<ProductViewModel>());
    }
}
