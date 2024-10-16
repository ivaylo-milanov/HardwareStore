namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Infrastructure.Models;
    using System.Diagnostics.CodeAnalysis;

    public class ProductOrderComparer : IEqualityComparer<ProductOrder>
    {
        public bool Equals(ProductOrder? x, ProductOrder? y)
        {
            if (x == null || y == null)
                return false;

            return x.OrderId.ToString() == y.OrderId.ToString()
                && x.ProductId == y.ProductId
                && x.Quantity == y.Quantity;
        }

        public int GetHashCode(ProductOrder obj)
        {
            return obj.OrderId.GetHashCode()
                ^ obj.ProductId.GetHashCode()
                ^ obj.Quantity.GetHashCode();
        }
    }
}
