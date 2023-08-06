namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Motherboard;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class MotherboardsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<MotherboardsController> logger;

        public MotherboardsController(IProductService productService, ILogger<MotherboardsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<MotherboardViewModel> model;
            try
            {
                model = await this.productService.GetModel<MotherboardViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterMotherboards([FromBody] MotherboardFilterOptions filter)
        {
            IEnumerable<MotherboardViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<MotherboardViewModel, MotherboardFilterOptions>(filter);
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