namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Search;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private readonly IMemoryCache memoryCache;
        private readonly IFilterService filterService;

        public SearchController(ISearchService searchService, IFilterService filterService, IMemoryCache memoryCache)
        {
            this.searchService = searchService;
            this.filterService = filterService;
            this.memoryCache = memoryCache;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var products = await this.searchService.GetProductsByKeyword(keyword);
            this.memoryCache.Set("Products", products);

            return RedirectToAction("Index");
        }

        public IActionResult FilterSearchedProducts([FromBody] SearchFilterOptions filter)
        {
            if (!this.memoryCache.TryGetValue("Products", out IEnumerable<SearchViewModel> products))
            {
                return BadRequest("Searched data not found.");
            }

            IEnumerable<SearchViewModel> filtered = this.filterService.FilterProducts(products, filter);

            return ViewComponent("ProductComponent", filtered);
        }
    }
}
