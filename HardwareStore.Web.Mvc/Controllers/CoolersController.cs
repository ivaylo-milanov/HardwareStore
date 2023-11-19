namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Cooler;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class CoolersController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<CoolersController> logger;

        public CoolersController(IProductService productService, ILogger<CoolersController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<CoolerViewModel> model;
            try
            {
                model = await this.productService.GetModel<CoolerViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterCoolers([FromBody] CoolerFilterOptions filter)
        {
            IEnumerable<CoolerViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<CoolerViewModel, CoolerFilterOptions>(filter);
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