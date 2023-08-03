namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Mouse;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class MousesController : Controller
    {
        private readonly IProductService productService;

        public MousesController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<MouseViewModel> model;
            try
            {
                model = await this.productService.GetModel<MouseViewModel>();
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        public IActionResult FilterMouses([FromBody] MouseFilterOptions filter)
        {
            IEnumerable<MouseViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<MouseViewModel, MouseFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}