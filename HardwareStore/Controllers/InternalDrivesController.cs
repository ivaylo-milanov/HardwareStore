namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.InternalDrive;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class InternalDrivesController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IProductService productService;

        public InternalDrivesController(IProductService productService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var internalDrives = await this.productService.GetProductsAsync<InternalDriveViewModel>();
            this.memoryCache.Set("Internal drives", internalDrives);

            return View(internalDrives);
        }

        public IActionResult FilterInternalDrives([FromBody] InternalDriveFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Internal drives", out IEnumerable<InternalDriveViewModel> internalDrives))
            {
                return BadRequest("Internal drives data not found.");
            }

            IEnumerable<InternalDriveViewModel> filtered = this.productService.FilterProducts(internalDrives, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
