namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Processor;
    using Microsoft.AspNetCore.Mvc;

    public class ProcessorsController : Controller
    {
        private readonly IProductService productService;

        public ProcessorsController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.productService.GetModel<ProcessorViewModel>();

            return View(model);
        }

        public IActionResult FilterProcessors([FromBody] ProcessorFilterOptions filter)
        {
            IEnumerable<ProcessorViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<ProcessorViewModel, ProcessorFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
