namespace HardwareStore.Core.Extensions
{
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Models;
    using System;

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

        public static IEnumerable<TModel> GetFilteredProducts<TModel, TFilter>(this IEnumerable<TModel> products, TFilter filter)
            where TModel : ProductViewModel
            where TFilter : ProductFilterOptions
        {
            var properties = filter.GetType().GetProperties();

            foreach (var property in properties)
            {
                var filterValue = property.GetValue(filter);

                if (filterValue != null)
                {
                    if (!(filterValue is IEnumerable<object> filterList))
                    {
                        continue;
                    }

                    if (filterList.Any())
                    {
                        products = products.Where(p => filterList.Contains(p.GetType().GetProperty(property.Name).GetValue(p)));
                    }
                }
            }

            return products;
        }
    }
}
