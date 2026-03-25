namespace HardwareStore.Core.ViewModels.Product
{
    /// <summary>
    /// Unified product row for category and search listings; options are stored as JSON on the entity.
    /// </summary>
    public class CatalogProductViewModel : ProductViewModel
    {
        public Dictionary<string, string> Options { get; set; } = new();
    }
}
