namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Case;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;

    public class CasesController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<CasesController> logger;

        public CasesController(IProductService productService, ILogger<CasesController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProductsViewModel<CaseViewModel> model;
            try
            {
                model = await this.productService.GetModel<CaseViewModel>();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult FilterCases([FromBody] CaseFilterOptions filter)
        {
            IEnumerable<CaseViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<CaseViewModel, CaseFilterOptions>(filter);
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
