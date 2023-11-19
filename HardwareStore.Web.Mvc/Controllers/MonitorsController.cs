namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Monitor;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class MonitorsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<MonitorsController> logger;

        public MonitorsController(IProductService productService, ILogger<MonitorsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<MonitorViewModel> model;
            try
            {
                model = await this.productService.GetModel<MonitorViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterMonitors([FromBody] MonitorFilterOptions filter)
        {
            IEnumerable<MonitorViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<MonitorViewModel, MonitorFilterOptions>(filter);
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