namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Processor;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class ProcessorsController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<ProcessorsController> logger;

        public ProcessorsController(IProductService productService, ILogger<ProcessorsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<ProcessorViewModel> model;
            try
            {
                model = await this.productService.GetModel<ProcessorViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterProcessors([FromBody] ProcessorFilterOptions filter)
        {
            IEnumerable<ProcessorViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<ProcessorViewModel, ProcessorFilterOptions>(filter);
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
