namespace HardwareStore.Core.ViewModels.Admin
{
    public class ProductListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string? ManufacturerName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
