namespace HardwareStore.Core.Services
{
    using System.Text.Json;
    using HardwareStore.Common;
    using HardwareStore.Core.Enums;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Core.ViewModels.Home;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        #region Fields and construction

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = null,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        private readonly IRepository repository;

        public ProductService(IRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region IProductService

        public Task<bool> CategoryExistsAsync(string categoryName) =>
            this.repository.AnyAsync<Category>(c => c.Name == categoryName);

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            var newestProducts = await this.repository.All<Product>()
                .OrderByDescending(p => p.AddDate)
                .Select(p => new ProductViewModel { Id = p.Id, Name = p.Name, Price = p.Price })
                .Take(4)
                .ToListAsync()
                .ConfigureAwait(false);

            var mostBoughtProducts = await this.repository.All<Product>(p => p.ProductsOrders.Count > 0)
                .OrderByDescending(p => p.ProductsOrders.Count)
                .Take(4)
                .Select(p => new ProductViewModel { Id = p.Id, Name = p.Name, Price = p.Price })
                .ToListAsync()
                .ConfigureAwait(false);

            return new HomeViewModel
            {
                NewestProducts = newestProducts,
                MostBoughtProducts = mostBoughtProducts,
            };
        }

        public async Task<ProductsViewModel<CatalogProductViewModel>> GetCategoryCatalogAsync(string categoryName)
        {
            var products = await LoadProductsForCategoryAsync(categoryName).ConfigureAwait(false);
            return BuildCatalogViewModel(products);
        }

        public async Task<ProductsViewModel<CatalogProductViewModel>> GetSearchCatalogAsync(string keyword)
        {
            var products = string.IsNullOrWhiteSpace(keyword)
                ? await LoadAllProductsForSearchAsync().ConfigureAwait(false)
                : await LoadSearchByKeywordAsync(keyword).ConfigureAwait(false);
            return BuildCatalogViewModel(products);
        }

        public async Task<IEnumerable<CatalogProductViewModel>> FilterCategoryCatalogAsync(string categoryName, string filterJson)
        {
            var products = await LoadProductsForCategoryAsync(categoryName).ConfigureAwait(false);
            var vms = products.Select(MapToCatalog);
            return ApplyParsedFilter(vms, ParseFilterJson(filterJson));
        }

        public async Task<IEnumerable<CatalogProductViewModel>> FilterSearchCatalogAsync(string keyword, string filterJson)
        {
            var products = string.IsNullOrWhiteSpace(keyword)
                ? await LoadAllProductsForSearchAsync().ConfigureAwait(false)
                : await LoadSearchByKeywordAsync(keyword).ConfigureAwait(false);
            var vms = products.Select(MapToCatalog);
            return ApplyParsedFilter(vms, ParseFilterJson(filterJson));
        }

        public async Task<ProductDetailsModel> GetProductDetails(int productId)
        {
            var product = await this.repository
                .All<Product>()
                .Include(p => p.Manufacturer)
                .Include(p => p.AssemblyComponents)
                    .ThenInclude(ac => ac.ComponentProduct)
                        .ThenInclude(c => c.Manufacturer)
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (product == null)
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);

            var opts = ReadOptionsDictionary(product);
            var assembly = product.AssemblyComponents
                .OrderBy(ac => ac.SortOrder)
                .ThenBy(ac => ac.Id)
                .Select(ac => new AssemblyComponentModel
                {
                    Role = ac.Role,
                    ProductId = ac.ComponentProductId,
                    Name = ac.ComponentProduct.Name,
                    ReferenceNumber = ac.ComponentProduct.ReferenceNumber,
                    Price = ac.ComponentProduct.Price,
                    Quantity = ac.Quantity,
                })
                .ToList();

            return new ProductDetailsModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                AddDate = product.AddDate,
                Manufacturer = product.Manufacturer?.Name!,
                ReferenceNumber = product.ReferenceNumber,
                Description = product.Description!,
                Warranty = product.Warranty,
                ImageUrl = "/images/product-placeholder.svg",
                Attributes = opts.Select(kv => new ProductAttributeExportModel { Name = kv.Key, Value = kv.Value }).ToList(),
                AssemblyComponents = assembly,
            };
        }

        public async Task<bool> IsProductInDbFavorites(string customerId, int productId)
        {
            var customer = await this.repository.FindAsync<Customer>(customerId).ConfigureAwait(false);
            if (customer == null)
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId).ConfigureAwait(false))
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);

            return await this.repository
                .AnyAsync<Favorite>(f => f.CustomerId == customerId && f.ProductId == productId)
                .ConfigureAwait(false);
        }

        #endregion

        #region Data loading

        private async Task<List<Product>> LoadProductsForCategoryAsync(string categoryName)
        {
            return await this.repository
                .All<Product>()
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Where(p => p.Category.Name == categoryName)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private async Task<List<Product>> LoadAllProductsForSearchAsync()
        {
            return await this.repository
                .All<Product>()
                .Include(p => p.Manufacturer)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private async Task<List<Product>> LoadSearchByKeywordAsync(string keyword)
        {
            var term = EscapeLikePattern(keyword.Trim());
            var pattern = $"%{term}%";

            var list = await this.repository
                .All<Product>()
                .Include(p => p.Manufacturer)
                .Where(p =>
                    EF.Functions.Like(p.Name, pattern) ||
                    (p.Description != null && EF.Functions.Like(p.Description, pattern)) ||
                    (p.Model != null && EF.Functions.Like(p.Model, pattern)) ||
                    (p.Manufacturer != null && EF.Functions.Like(p.Manufacturer.Name, pattern)))
                .ToListAsync()
                .ConfigureAwait(false);

            return list;
        }

        private static string EscapeLikePattern(string s) =>
            s.Replace("[", "[[]", StringComparison.Ordinal)
                .Replace("%", "[%]", StringComparison.Ordinal)
                .Replace("_", "[_]", StringComparison.Ordinal);

        #endregion

        #region Catalog building

        private static CatalogProductViewModel MapToCatalog(Product p)
        {
            var opts = ReadOptionsDictionary(p);
            return new CatalogProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                AddDate = p.AddDate,
                Manufacturer = p.Manufacturer?.Name ?? string.Empty,
                Options = opts,
            };
        }

        private static Dictionary<string, string> ReadOptionsDictionary(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.Options) || p.Options == "{}")
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(p.Options, JsonOptions);
                if (parsed != null && parsed.Count > 0)
                    return new Dictionary<string, string>(parsed, StringComparer.OrdinalIgnoreCase);
            }
            catch (JsonException)
            {
                // ignore invalid json
            }

            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        private static ProductsViewModel<CatalogProductViewModel> BuildCatalogViewModel(IEnumerable<Product> rows)
        {
            var vms = rows.Select(MapToCatalog).ToList();
            return new ProductsViewModel<CatalogProductViewModel>
            {
                Products = vms,
                Filters = BuildFilterCategories(vms),
            };
        }

        private static IEnumerable<FilterCategoryModel> BuildFilterCategories(IReadOnlyList<CatalogProductViewModel> products)
        {
            var filters = new List<FilterCategoryModel>();

            var mfrValues = products
                .Select(p => p.Manufacturer)
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .ToList();
            var mfrSplit = string.Join(", ", mfrValues)
                .Split(", ")
                .Select(v => v.Trim())
                .Where(v => v.Length > 0)
                .Distinct();
            filters.Add(new FilterCategoryModel
            {
                Name = "Manufacturer",
                Title = "Manufacturer",
                Values = mfrSplit.FirstOrDefault() == string.Empty ? new List<string>() : mfrSplit.OrderByDescending(v => v),
            });

            foreach (var key in products.SelectMany(p => p.Options.Keys).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var values = products
                    .Select(p => p.Options.TryGetValue(key, out var v) ? v : null)
                    .Where(v => v != null)
                    .Cast<string>()
                    .Distinct()
                    .ToList();

                var splitValues = string.Join(", ", values)
                    .Split(", ")
                    .Select(v => v.Trim())
                    .Distinct();

                filters.Add(new FilterCategoryModel
                {
                    Name = key,
                    Title = key,
                    Values = splitValues.FirstOrDefault() == string.Empty
                        ? new List<string>()
                        : splitValues.OrderByDescending(v => v),
                });
            }

            return filters;
        }

        #endregion

        #region Filtering and ordering

        private static IEnumerable<CatalogProductViewModel> ApplyParsedFilter(
            IEnumerable<CatalogProductViewModel> products,
            ParsedFilter filter)
        {
            var q = products;
            if (filter.Manufacturer is { Count: > 0 })
                q = q.Where(p => MatchesFilter(filter.Manufacturer, p.Manufacturer));

            foreach (var kv in filter.OptionSelections)
            {
                var key = kv.Key;
                var selected = kv.Value;
                if (selected == null || selected.Count == 0)
                    continue;
                q = q.Where(p =>
                    p.Options.TryGetValue(key, out var val) && MatchesFilter(selected, val));
            }

            return OrderBySelection(q, filter.Order);
        }

        private static bool MatchesFilter(IEnumerable<string> selectedValues, object? productField)
        {
            if (productField == null)
                return false;

            var text = productField.ToString()!;
            return selectedValues.Any(selected => text.Contains(selected, StringComparison.Ordinal));
        }

        private static IEnumerable<CatalogProductViewModel> OrderBySelection(IEnumerable<CatalogProductViewModel> products, int order)
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

        #endregion

        #region Filter JSON parsing

        private static ParsedFilter ParseFilterJson(string json)
        {
            var result = new ParsedFilter();
            if (string.IsNullOrWhiteSpace(json))
                return result;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            foreach (var prop in root.EnumerateObject())
            {
                if (string.Equals(prop.Name, "Order", StringComparison.OrdinalIgnoreCase))
                {
                    result.Order = prop.Value.ValueKind == JsonValueKind.String
                        ? int.Parse(prop.Value.GetString() ?? "1")
                        : prop.Value.GetInt32();
                    continue;
                }

                if (string.Equals(prop.Name, "Manufacturer", StringComparison.OrdinalIgnoreCase))
                {
                    result.Manufacturer = ToStringList(prop.Value);
                    continue;
                }

                var list = ToStringList(prop.Value);
                if (list is { Count: > 0 })
                    result.OptionSelections[prop.Name] = list;
            }

            return result;
        }

        private static List<string>? ToStringList(JsonElement el)
        {
            return el.ValueKind switch
            {
                JsonValueKind.Array => el.EnumerateArray().Select(e => e.GetString()).Where(s => s != null).Cast<string>().ToList(),
                JsonValueKind.String => new List<string> { el.GetString()! },
                _ => null,
            };
        }

        private sealed class ParsedFilter
        {
            public int Order { get; set; } = 1;

            public List<string>? Manufacturer { get; set; }

            public Dictionary<string, List<string>> OptionSelections { get; } = new(StringComparer.OrdinalIgnoreCase);
        }

        #endregion
    }
}
