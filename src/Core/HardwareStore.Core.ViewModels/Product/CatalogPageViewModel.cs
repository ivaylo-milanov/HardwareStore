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

        /// <summary>Sort order (<see cref="Enums.ProductOrdering"/> value as int).</summary>
        public int SelectedOrder { get; set; } = 1;

        /// <summary>Selected filter checkbox values by filter name (e.g. Manufacturer).</summary>
        public IReadOnlyDictionary<string, IReadOnlyList<string>> SelectedFilterValues { get; set; } =
            new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);

        public ProductsViewModel<CatalogProductViewModel> Catalog { get; set; } = null!;
    }
}
