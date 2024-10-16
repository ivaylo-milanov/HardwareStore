namespace HardwareStore.Core.ViewModels.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public ICollection<ShoppingCartItemViewModel> Shoppings { get; set; } = null!;

        public decimal TotalCartPrice { get; set; }
}
}
