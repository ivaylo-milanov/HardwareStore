namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductExportModel : ProductViewModel
    {
        public IEnumerable<ProductAttributeExportModel> Attributes { get; set; } = null!;
    }
}
