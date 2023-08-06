namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.PowerSupply;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class PowerSuppliesController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<PowerSuppliesController> logger;

        public PowerSuppliesController(IProductService productService, ILogger<PowerSuppliesController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<PowerSupplyViewModel> model;
            try
            {
                model = await this.productService.GetModel<PowerSupplyViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterPowerSupplies([FromBody] PowerSupplyFilterOptions filter)
        {
            IEnumerable<PowerSupplyViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<PowerSupplyViewModel, PowerSupplyFilterOptions>(filter);
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
