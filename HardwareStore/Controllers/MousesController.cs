namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Mouse;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class MousesController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<MousesController> logger;

        public MousesController(IProductService productService, ILogger<MousesController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<MouseViewModel> model;
            try
            {
                model = await this.productService.GetModel<MouseViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterMouses([FromBody] MouseFilterOptions filter)
        {
            IEnumerable<MouseViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<MouseViewModel, MouseFilterOptions>(filter);
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