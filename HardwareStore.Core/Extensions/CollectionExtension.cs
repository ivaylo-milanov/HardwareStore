namespace HardwareStore.Extensions
{
    using Dropbox.Api.Users;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Numerics;
    using static Dropbox.Api.Files.SearchMatchType;
    using System.Reflection;

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

        public static IEnumerable<ProductNameValueModel> GetDistinctValues<T>(this IEnumerable<T> products,string propertyName) where T : ProductViewModel
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo != null)
            {
                var collection = products
                    .Select(m => new ProductNameValueModel
                    {
                        Name = propertyInfo.GetValue(m)?.ToString(),
                        Value = propertyInfo.GetValue(m)?.ToString()
                    })
                    .DistinctBy(m => m.Name)
                    .ToList();

                collection.Insert(0, new ProductNameValueModel
                {
                    Name = "All",
                    Value = "All"
                });

                return collection;
            }

            return Enumerable.Empty<ProductNameValueModel>();
        }
    }
}
