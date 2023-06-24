namespace HardwareStore.Extensions
{
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Models;
    using System.Numerics;

    public static class CollectionExtension
    {
        public static string GetAttributeValue(this ICollection<ProductAttribute> attributes, string key)
            => attributes.FirstOrDefault(a => a.Name == key)!.Value;

        public static IEnumerable<T> OrderProducts<T>(this IEnumerable<T> products, string order) where T : ProductViewModel
        {
            switch (order)
            {
                case "Highest price":
                    products = products.OrderByDescending(m => m.Price);
                    break;
                case "Lowest price":
                    products = products.OrderBy(m => m.Price);
                    break;
                case "Oldest":
                    products = products.OrderBy(m => m.AddDate);
                    break;
                case "Newest":
                    products = products.OrderByDescending(m => m.AddDate);
                    break;
            }

            return products;
        }
    }
}
