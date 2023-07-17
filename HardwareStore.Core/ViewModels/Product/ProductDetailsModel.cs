namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductDetailsModel : ProductViewModel
    {
        public string Description { get; set; } = null!;

        public string ReferenceNumber { get; set; } = null!;

        public int Warranty { get; set; }

        public IEnumerable<ProductAttributeExportModel> Attributes { get; set; } = null!;
    }
}
