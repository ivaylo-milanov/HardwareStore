namespace HardwareStore.Core.Services
{
    using System.Reflection;
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.Infrastructure.Enum;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly IRepository repository;

        public ProductService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ProductsViewModel<TModel>> GetModel<TModel>(string? keyword = null)
            where TModel : ProductViewModel
        {
            var products = await GetProductsAsync<TModel>(keyword).ConfigureAwait(false);
            return BuildProductsViewModel(products);
        }

        public async Task<IEnumerable<TModel>> FilterProductsAsync<TModel, TFilter>(TFilter filter)
            where TModel : ProductViewModel
            where TFilter : ProductFilterOptions
        {
            var products = await GetProductsAsync<TModel>().ConfigureAwait(false);
            return ApplyFilters(products, filter);
        }

        public async Task<IEnumerable<SearchViewModel>> FilterSearchProductsAsync(string keyword, SearchFilterOptions filter)
        {
            var products = await GetProductsAsync<SearchViewModel>(keyword).ConfigureAwait(false);
            return ApplyFilters(products, filter);
        }

        private static ProductsViewModel<TModel> BuildProductsViewModel<TModel>(IEnumerable<TModel> products)
            where TModel : ProductViewModel
        {
            return new ProductsViewModel<TModel>
            {
                Filters = BuildFilterCategories(products),
                Products = products,
            };
        }

        private static IEnumerable<TModel> ApplyFilters<TModel, TFilter>(IEnumerable<TModel> products, TFilter filter)
            where TModel : ProductViewModel
            where TFilter : ProductFilterOptions
        {
            foreach (var prop in filter.GetType().GetProperties())
            {
                var raw = prop.GetValue(filter);
                if (raw is not IEnumerable<string> list)
                    continue;

                products = products.Where(p =>
                    MatchesFilter(list, p.GetType().GetProperty(prop.Name)?.GetValue(p)));
            }

            return OrderBySelection(products, filter.Order);
        }

        private static bool MatchesFilter(IEnumerable<string> selectedValues, object? productField)
        {
            if (productField == null)
                return false;

            var text = productField.ToString()!;
            return selectedValues.Any(selected => text.Contains(selected));
        }

        private static IEnumerable<TModel> OrderBySelection<TModel>(IEnumerable<TModel> products, int order)
            where TModel : ProductViewModel
        {
            return (ProductOrdering)order switch
            {
                ProductOrdering.LowestPrice => products.OrderBy(p => p.Price),
                ProductOrdering.HighestPrice => products.OrderByDescending(p => p.Price),
                ProductOrdering.Newest => products.OrderByDescending(p => p.AddDate),
                ProductOrdering.Oldest => products.OrderBy(p => p.AddDate),
                _ => products,
            };
        }

        /// <summary>
        /// Loads products for a category view model, or for <see cref="SearchViewModel"/> when <paramref name="keyword"/> is used (including empty = all products).
        /// </summary>
        private async Task<IEnumerable<TModel>> GetProductsAsync<TModel>(string? keyword = null)
            where TModel : ProductViewModel
        {
            if (typeof(TModel) == typeof(SearchViewModel))
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                    return (IEnumerable<TModel>)(object)await LoadSearchByKeywordAsync(keyword).ConfigureAwait(false);

                return (IEnumerable<TModel>)(object)await LoadAllForSearchAsync().ConfigureAwait(false);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("A search keyword only applies to SearchViewModel.", nameof(keyword));

            return await LoadByCategoryAsync<TModel>().ConfigureAwait(false);
        }

        private async Task<IEnumerable<TModel>> LoadByCategoryAsync<TModel>()
            where TModel : ProductViewModel
        {
            var categoryAttr = typeof(TModel).GetCustomAttribute<CategoryAttribute>();
            if (categoryAttr == null)
                throw new ArgumentException($"The model type {typeof(TModel).Name} does not have a Category attribute");

            var rows = await this.repository
                .All<Product>()
                .Include(p => p.Manufacturer)
                .Include(p => p.Characteristics)
                .ThenInclude(c => c.CharacteristicName)
                .Where(p => p.Category.Name == categoryAttr.CategoryName)
                .Select(p => new ProductExportModel
                {
                    Id = p.Id,
                    Price = p.Price,
                    Name = p.Name,
                    AddDate = p.AddDate,
                    Manufacturer = p.Manufacturer!.Name,
                    Attributes = p.Characteristics
                        .Select(pa => new ProductAttributeExportModel
                        {
                            Name = pa.CharacteristicName.Name,
                            Value = pa.Value,
                        })
                        .ToList(),
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return rows.Select(MapToViewModel<TModel>).ToList();
        }

        private async Task<IEnumerable<SearchViewModel>> LoadAllForSearchAsync()
        {
            return await this.repository.All<Product>()
                .Include(p => p.Manufacturer)
                .Select(p => new SearchViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Manufacturer = p.Manufacturer!.Name,
                    AddDate = p.AddDate,
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private async Task<IEnumerable<SearchViewModel>> LoadSearchByKeywordAsync(string keyword)
        {
            const string sql = @"
                SELECT p.* 
                FROM Products p
                LEFT JOIN Manufacturers m ON m.Id = p.ManufacturerId
                WHERE (
                    FREETEXT((p.Name, p.Description, p.Model), @keyword) OR
                    FREETEXT((m.Name), @keyword)
                )";

            var matches = await this.repository
                .FromSqlRawAsync<Product>(sql, new SqlParameter("@keyword", keyword));

            var productIds = matches.Select(p => p.Id).ToList();

            var manufacturers = await this.repository.All<Manufacturer>()
                .Where(m => m.Products.Any(p => productIds.Contains(p.Id)))
                .ToDictionaryAsync(m => m.Id, m => m.Name)
                .ConfigureAwait(false);

            return matches
                .Select(p => new SearchViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Manufacturer = p.ManufacturerId is { } mid && manufacturers.TryGetValue(mid, out var name)
                        ? name
                        : null!,
                    AddDate = p.AddDate,
                })
                .ToList();
        }

        private static TModel MapToViewModel<TModel>(ProductExportModel product)
            where TModel : ProductViewModel
        {
            var model = Activator.CreateInstance<TModel>();

            foreach (var prop in typeof(TModel).GetProperties())
            {
                var ch = prop.GetCustomAttribute<CharacteristicAttribute>();
                if (ch != null)
                {
                    var attrName = ch.Name ?? prop.Name;
                    object? value = prop.Name == "Manufacturer"
                        ? product.Manufacturer
                        : product.Attributes.FirstOrDefault(a => a.Name == attrName)?.Value;

                    if (value == null && prop.Name != "Manufacturer")
                        continue;

                    if (value != null && prop.PropertyType != value.GetType())
                        value = Convert.ChangeType(value, prop.PropertyType);

                    prop.SetValue(model, value);
                }
                else
                {
                    var src = typeof(ProductExportModel).GetProperty(prop.Name);
                    if (src != null)
                        prop.SetValue(model, src.GetValue(product));
                }
            }

            return model;
        }

        private static IEnumerable<FilterCategoryModel> BuildFilterCategories<TModel>(IEnumerable<TModel> products)
            where TModel : ProductViewModel
        {
            var filters = new List<FilterCategoryModel>();

            foreach (var prop in typeof(TModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(CharacteristicAttribute))))
            {
                var values = products
                    .Select(m => prop.GetValue(m)?.ToString())
                    .Where(v => v != null)
                    .Cast<string>()
                    .Distinct()
                    .ToList();

                var splitValues = string.Join(", ", values)
                    .Split(", ")
                    .Select(v => v.Trim())
                    .Distinct();

                var ch = prop.GetCustomAttribute<CharacteristicAttribute>();
                var title = ch?.Name ?? prop.Name;

                filters.Add(new FilterCategoryModel
                {
                    Name = prop.Name,
                    Title = title,
                    Values = splitValues.FirstOrDefault() == string.Empty
                        ? new List<string>()
                        : splitValues.OrderByDescending(v => v),
                });
            }

            return filters;
        }
    }
}
