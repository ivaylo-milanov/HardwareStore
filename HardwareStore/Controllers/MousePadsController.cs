namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.MousePad;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class MousePadsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<MousePadsController> logger;

        public MousePadsController(IProductService productService, ILogger<MousePadsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<MousePadViewModel> model;
            try
            {
                model = await this.productService.GetModel<MousePadViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterMousePads([FromBody] MousePadFilterOptions filter)
        {
            IEnumerable<MousePadViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<MousePadViewModel, MousePadFilterOptions>(filter);
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
