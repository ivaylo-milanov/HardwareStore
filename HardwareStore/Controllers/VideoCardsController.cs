namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.VideoCard;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Route("/video-cards")]
    public class VideoCardsController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public VideoCardsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var videoCards = await this.productService.GetProductsAsync<VideoCardViewModel>();
            this.memoryCache.Set("Video cards", videoCards);

            return View(videoCards);
        }

        public IActionResult FilterVideoCards([FromBody] VideoCardFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Video cards", out IEnumerable<VideoCardViewModel> videoCards))
            {
                return BadRequest("Video cards data not found.");
            }

            IEnumerable<VideoCardViewModel> filtered = this.productService.FilterProducts(videoCards, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
