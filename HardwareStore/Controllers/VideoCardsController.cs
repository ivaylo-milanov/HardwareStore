namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.VideoCard;
    using Microsoft.AspNetCore.Mvc;

    public class VideoCardsController : Controller
    {
        private readonly IProductService productService;

        public VideoCardsController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.productService.GetModel<VideoCardViewModel>();

            return View(model);
        }

        public IActionResult FilterVideoCards([FromBody] VideoCardFilterOptions filter)
        {
            IEnumerable<VideoCardViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<VideoCardViewModel, VideoCardFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
