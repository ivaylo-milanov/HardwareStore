namespace HardwareStore.Core.ViewModels.Product
{
    /// <summary>Product category listing or search results page (typed replacement for ViewBag).</summary>
    public class CatalogPageViewModel
    {
        public string PageTitle { get; set; } = null!;

        /// <summary>Category route value for product filter POST.</summary>
        public string? CategoryKey { get; set; }

        /// <summary>Search keyword for search filter POST.</summary>
        public string? SearchKeyword { get; set; }

        public ProductsViewModel<CatalogProductViewModel> Catalog { get; set; } = null!;
    }
}
