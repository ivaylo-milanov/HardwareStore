namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.VideoCard;
    using Microsoft.AspNetCore.Mvc;

    public class VideoCardsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<VideoCardsController> logger;

        public VideoCardsController(IProductService productService, ILogger<VideoCardsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<VideoCardViewModel> model;
            try
            {
                model = await this.productService.GetModel<VideoCardViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterVideoCards([FromBody] VideoCardFilterOptions filter)
        {
            IEnumerable<VideoCardViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<VideoCardViewModel, VideoCardFilterOptions>(filter);
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
