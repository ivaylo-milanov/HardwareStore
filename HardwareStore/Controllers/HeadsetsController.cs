namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class HeadsetsController : Controller
    {
        private readonly IHeadsetService headsetService;
        private readonly IMemoryCache memoryCache;

        public HeadsetsController(IHeadsetService headsetService, IMemoryCache memoryCache)
        {
            this.headsetService = headsetService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var headsets = await this.headsetService.GetAllProducts();
            this.memoryCache.Set("Headsets", headsets);

            return View(headsets);
        }

        public IActionResult FilterHeadsets([FromBody] HeadsetFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Headsets", out IEnumerable<HeadsetViewModel> headsets))
            {
                return BadRequest("Keyboards data not found.");
            }

            IEnumerable<HeadsetViewModel> filtered = headsets;
            if (filter != null)
            {
                filtered = filtered.GetFilteredProducts(filter);
            }

            return PartialView("_ProductPartialView", filtered);
        }
    }
}
