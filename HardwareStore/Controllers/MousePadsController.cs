namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.MousePad;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class MousePadsController : Controller
    {
        private readonly IProductService productService;

        public MousePadsController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<MousePadViewModel> model;
            try
            {
                model = await this.productService.GetModel<MousePadViewModel>();
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        public IActionResult FilterMousePads([FromBody] MousePadFilterOptions filter)
        {
            IEnumerable<MousePadViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<MousePadViewModel, MousePadFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
