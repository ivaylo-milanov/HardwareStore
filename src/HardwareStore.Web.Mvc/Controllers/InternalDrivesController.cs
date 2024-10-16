namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.InternalDrive;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class InternalDrivesController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<InternalDrivesController> logger;

        public InternalDrivesController(IProductService productService, ILogger<InternalDrivesController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<InternalDriveViewModel> model;
            try
            {
                model = await this.productService.GetModel<InternalDriveViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterInternalDrives([FromBody] InternalDriveFilterOptions filter)
        {
            IEnumerable<InternalDriveViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<InternalDriveViewModel, InternalDriveFilterOptions>(filter);
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
