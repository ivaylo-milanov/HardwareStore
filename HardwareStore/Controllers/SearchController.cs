namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Search;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : Controller
    {
        private readonly IProductService productService;

        public SearchController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keyword)
        {
            var model = await this.productService.GetSearchModel(keyword);

            return View(model);
        }

        public IActionResult FilterSearchedProducts([FromBody] SearchFilterOptions filter)
        {
            IEnumerable<SearchViewModel> filtered;
            try
            {
                filtered = this.productService.FilterProducts<SearchViewModel, SearchFilterOptions>(filter);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return PartialView("_ProductsPartialView", filtered);
        }
    }
}
