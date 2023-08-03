namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Cooler;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class CoolersController : Controller
    {
        private readonly IProductService productService;

        public CoolersController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<CoolerViewModel> model;
            try
            {
                model = await this.productService.GetModel<CoolerViewModel>();
            }
            catch (Exception)
            {
                throw;
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
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}