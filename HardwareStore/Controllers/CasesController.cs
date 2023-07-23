namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Case;
    using Microsoft.AspNetCore.Mvc;

    public class CasesController : Controller
    {
        private readonly IProductService productService;

        public CasesController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.productService.GetModel<CaseViewModel>();

            return View(model);
        }

        public IActionResult FilterCases([FromBody] CaseFilterOptions filter)
        {
            IEnumerable<CaseViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<CaseViewModel, CaseFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
