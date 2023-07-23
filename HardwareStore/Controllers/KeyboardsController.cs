namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Keyboard;
    using Microsoft.AspNetCore.Mvc;

    public class KeyboardsController : Controller
    {
        private readonly IProductService productService;

        public KeyboardsController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.productService.GetModel<KeyboardViewModel>();

            return View(model);
        }

        public IActionResult FilterKeyboards([FromBody] KeyboardFilterOptions filter)
        {
            IEnumerable<KeyboardViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<KeyboardViewModel, KeyboardFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
