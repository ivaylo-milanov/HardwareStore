namespace HardwareStore.Core.ViewModels.ShoppingCart
{
    public class ShoppingCartItemViewModel : ShoppingCartExportModel
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
