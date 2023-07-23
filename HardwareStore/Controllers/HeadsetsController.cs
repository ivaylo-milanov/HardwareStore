namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using Microsoft.AspNetCore.Mvc;

    public class HeadsetsController : Controller
    {
        private readonly IProductService productService;

        public HeadsetsController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.productService.GetModel<HeadsetViewModel>();

            return View(model);
        }

        public IActionResult FilterHeadsets([FromBody] HeadsetFilterOptions filter)
        {
            IEnumerable<HeadsetViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<HeadsetViewModel, HeadsetFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}