namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using System.Diagnostics.CodeAnalysis;

    public class ShoppingCartItemViewModelComparer : IEqualityComparer<ShoppingCartItemViewModel>
    {
        public bool Equals(ShoppingCartItemViewModel? x, ShoppingCartItemViewModel? y)
        {
            if (x == null || y == null)
                return false;

            return x.ProductId == y.ProductId
                && x.Quantity == y.Quantity
                && x.ProductQuantity == y.ProductQuantity
                && x.TotalPrice == y.TotalPrice
                && x.Name == y.Name
                && x.Price == y.Price;
        }

        public int GetHashCode([DisallowNull] ShoppingCartItemViewModel obj)
            => obj.ProductId.GetHashCode()
            ^ obj.Quantity.GetHashCode()
            ^ obj.ProductQuantity.GetHashCode()
            ^ obj.TotalPrice.GetHashCode()
            ^ obj.Name.GetHashCode()
            ^ obj.Price.GetHashCode();
    }
}
