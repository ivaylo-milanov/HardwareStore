namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Processor;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class ProcessorsController : Controller
    {
        private readonly IProductService productService;
        private readonly IMemoryCache memoryCache;

        public ProcessorsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var processors = await this.productService.GetProductsAsync<ProcessorViewModel>();
            this.memoryCache.Set("Processors", processors);

            return View(processors);
        }

        public IActionResult FilterProcessors([FromBody] ProcessorFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Processors", out IEnumerable<ProcessorViewModel> processors))
            {
                return BadRequest("Processors data not found.");
            }

            IEnumerable<ProcessorViewModel> filtered = this.productService.FilterProducts(processors, filter);

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
