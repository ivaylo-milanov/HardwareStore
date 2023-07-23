namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.InternalDrive;
    using Microsoft.AspNetCore.Mvc;

    public class InternalDrivesController : Controller
    {
        private readonly IProductService productService;

        public InternalDrivesController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.productService.GetModel<InternalDriveViewModel>();

            return View(model);
        }

        public IActionResult FilterInternalDrives([FromBody] InternalDriveFilterOptions filter)
        {
            IEnumerable<InternalDriveViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<InternalDriveViewModel, InternalDriveFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
