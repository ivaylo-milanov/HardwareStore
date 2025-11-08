namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IDetailsService detailsService;
        private readonly IProductService productService;
        private readonly ILogger<ProductController> logger;

        public ProductController(IDetailsService detailsService, ILogger<ProductController> logger, IProductService productService)
        {
            this.logger = logger;
            this.detailsService = detailsService;
            this.productService = productService;
        }

        public async Task<IActionResult> Index(string category)
        {
            try
            {
                var viewModelType = Type.GetType($"HardwareStore.Core.ViewModels.Details.{category}ViewModel", throwOnError: false, ignoreCase: true);

                if (viewModelType == null)
                {
                    this.logger.LogWarning("Unknown category: {Category}", category);
                    return RedirectToAction("Error", "Home", new { message = "Invalid category." });
                }

                var method = this.productService.GetType().GetMethod("GetModel")!.MakeGenericMethod(viewModelType);
                var task = (Task)method.Invoke(this.productService, null)!;
                await task.ConfigureAwait(false);

                var resultProperty = task.GetType().GetProperty("Result");
                ViewBag.Model = resultProperty?.GetValue(task)!;
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
            
            return View();
        }

        public IActionResult Filter([FromBody] object filter, string category)
        {
            IEnumerable<object> filtered;
            try
            {
                // Dynamically determine the ViewModel and FilterOptions types based on the category
                var viewModelType = Type.GetType($"HardwareStore.Core.ViewModels.Details.{category}ViewModel", throwOnError: false, ignoreCase: true);
                var filterOptionsType = Type.GetType($"HardwareStore.Core.ViewModels.Filters.{category}FilterOptions", throwOnError: false, ignoreCase: true);

                if (viewModelType == null || filterOptionsType == null)
                {
                    this.logger.LogWarning("Unknown category or filter options: {Category}", category);
                    return RedirectToAction("Error", "Home", new { message = "Invalid category or filter options." });
                }

                // Deserialize the filter object into the correct filter options type
                var deserializedFilter = System.Text.Json.JsonSerializer.Deserialize(filter.ToString()!, filterOptionsType);

                // Use reflection to call the generic method with the determined types
                var method = this.productService.GetType()?
                    .GetMethod("FilterProducts")?
                    .MakeGenericMethod(viewModelType, filterOptionsType);

                filtered = (IEnumerable<object>)method?.Invoke(this.productService, new[] { deserializedFilter })!;
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return PartialView("_ProductsPartialView", filtered);
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
    }
}
