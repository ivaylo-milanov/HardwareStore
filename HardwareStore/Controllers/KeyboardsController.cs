namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Keyboard;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class KeyboardsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<KeyboardsController> logger;

        public KeyboardsController(IProductService productService, ILogger<KeyboardsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<KeyboardViewModel> model;
            try
            {
                model = await this.productService.GetModel<KeyboardViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterKeyboards([FromBody] KeyboardFilterOptions filter)
        {
            IEnumerable<KeyboardViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<KeyboardViewModel, KeyboardFilterOptions>(filter);
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
