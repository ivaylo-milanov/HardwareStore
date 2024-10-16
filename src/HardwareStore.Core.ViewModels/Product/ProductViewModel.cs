namespace HardwareStore.Core.ViewModels.Product
{
    using HardwareStore.Core.Infrastructure.Attributes;

    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime AddDate { get; set; }

        [Characteristic]
        public string Manufacturer { get; set; } = null!;
    }
}
