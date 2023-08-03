namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Ram;
    using Microsoft.AspNetCore.Mvc;

    public class RamController : Controller
    {
        private readonly IProductService productService;

        public RamController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<RamViewModel> model;
            try
            {
                model = await this.productService.GetModel<RamViewModel>();
            }
            catch (Exception)
            {
                throw;
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
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
