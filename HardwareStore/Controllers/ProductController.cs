namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private readonly IDetailsService detailsService;
        private readonly ILogger<ProductController> logger;

        public ProductController(IDetailsService detailsService, ILogger<ProductController> logger)
        {
            this.logger = logger;
            this.detailsService = detailsService;
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
