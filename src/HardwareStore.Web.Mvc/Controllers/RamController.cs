namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Ram;
    using Microsoft.AspNetCore.Mvc;

    public class RamController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<RamController> logger;

        public RamController(IProductService productService, ILogger<RamController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<RamViewModel> model;
            try
            {
                model = await this.productService.GetModel<RamViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterRam([FromBody] RamFilterOptions filter)
        {
            IEnumerable<RamViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<RamViewModel, RamFilterOptions>(filter);
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
