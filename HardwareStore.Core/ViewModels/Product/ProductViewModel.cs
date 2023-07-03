namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime AddDate { get; set; }

        public string Manufacturer { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
