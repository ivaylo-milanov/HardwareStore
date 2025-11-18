using HardwareStore.Core.ViewModels.Product;

namespace HardwareStore.Core.ViewModels.Details
{
    public class ProductDetailsModel : ProductViewModel
    {
        public string Description { get; set; } = null!;

        public string ReferenceNumber { get; set; } = null!;

        public int Warranty { get; set; }

        public bool IsFavorite { get; set; }

        public IEnumerable<ProductAttributeExportModel> Attributes { get; set; } = null!;
    }
}
