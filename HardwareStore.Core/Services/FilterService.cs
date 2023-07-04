namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;

    public class FilterService : IFilterService
    {
        public IEnumerable<TModel> FilterProducts<TModel, TFilter>(IEnumerable<TModel> products, TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel
        {
            var properties = filter
                .GetType()
                .GetProperties()
                .Where(p => p.GetValue(filter) != null && typeof(IEnumerable<string>).IsAssignableFrom(p.GetValue(filter).GetType()));

            foreach (var property in properties)
            {
                var value = property.GetValue(filter);
                var list = value as IEnumerable<string>;

                products = products.Where(p => list.Contains(p.GetType().GetProperty(property.Name).GetValue(p)));
            }

            return products;
        }

        public IEnumerable<T> OrderProducts<T>(IEnumerable<T> products, string order) where T : ProductViewModel
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
