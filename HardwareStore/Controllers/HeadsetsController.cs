namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class HeadsetsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<HeadsetsController> logger;

        public HeadsetsController(IProductService productService, ILogger<HeadsetsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<HeadsetViewModel> model;
            try
            {
                model = await this.productService.GetModel<HeadsetViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterHeadsets([FromBody] HeadsetFilterOptions filter)
        {
            IEnumerable<HeadsetViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<HeadsetViewModel, HeadsetFilterOptions>(filter);
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