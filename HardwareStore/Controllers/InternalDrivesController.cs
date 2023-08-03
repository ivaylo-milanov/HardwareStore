namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.InternalDrive;
    using HardwareStore.Core.ViewModels.Product;
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
            ProductsViewModel<InternalDriveViewModel> model;
            try
            {
                model = await this.productService.GetModel<InternalDriveViewModel>();
            }
            catch (Exception)
            {
                throw;
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
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
