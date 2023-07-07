namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Processor;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class ProcessorsController : Controller
    {
        private readonly IProcessorService processorService;
        private readonly IMemoryCache memoryCache;
        private readonly IFilterService filterService;

        public ProcessorsController(
            IProcessorService processorService,
            IMemoryCache memoryCache,
            IFilterService filterService)
        {
            this.processorService = processorService;
            this.memoryCache = memoryCache;
            this.filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            var processors = await this.processorService.GetAllProducts();
            this.memoryCache.Set("Processors", processors);

            return View(processors);
        }

        public IActionResult FilterProcessors([FromBody] ProcessorFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Processors", out IEnumerable<ProcessorViewModel> processors))
            {
                return BadRequest("Processors data not found.");
            }

            IEnumerable<ProcessorViewModel> filtered = this.filterService.FilterProducts(processors, filter);

            return ViewComponent("ProductsComponent", filtered);
        }
    }
}
