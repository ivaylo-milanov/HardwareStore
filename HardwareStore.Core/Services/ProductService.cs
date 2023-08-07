namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.Enum;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System.Linq;
    using System.Reflection;

    public class ProductService : IProductService
    {
        private readonly IRepository repository;
        private readonly IMemoryCache memoryCache;

        public ProductService(IRepository repository, IMemoryCache memoryCache)
        {
            this.repository = repository;
            this.memoryCache = memoryCache;
        }

        public IEnumerable<TModel> FilterProducts<TModel, TFilter>(TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel
        {
            if (!this.memoryCache.TryGetValue("Products", out IEnumerable<TModel> products))
            {
                throw new ArgumentNullException(ExceptionMessages.NoProductsFound);
            }

            var properties = filter
                .GetType()
                .GetProperties()
                .Where(p => p.GetValue(filter) != null && typeof(IEnumerable<string>).IsAssignableFrom(p.GetValue(filter).GetType()));

            foreach (var property in properties)
            {
                var value = property.GetValue(filter);
                var list = value as IEnumerable<string>;

                products = products.Where(p => IsValid(list, p.GetType().GetProperty(property.Name).GetValue(p)));
            }

            products = this.OrderProducts(products, filter.Order);

            return products;
        }

        private bool IsValid(IEnumerable<string> list, object value)
        {
            bool isValid = false;


            if (value == null)
            {
                return false;
            }

            foreach (var item in list)
            {
                if (value.ToString().Contains(item))
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }

        private IEnumerable<TModel> OrderProducts<TModel>(IEnumerable<TModel> products, int order) where TModel : ProductViewModel
        {
            ProductOrdering ordering = (ProductOrdering)order;

            var orderedProducts = ordering switch
            {
                ProductOrdering.LowestPrice => products.OrderBy(p => p.Price),
                ProductOrdering.HighestPrice => products.OrderByDescending(p => p.Price),
                ProductOrdering.Newest => products.OrderByDescending(p => p.AddDate),
                ProductOrdering.Oldest => products.OrderBy(p => p.AddDate),
                _=> products
            };

            return orderedProducts;
        }

        private async Task<IEnumerable<TModel>> GetProductsAsync<TModel>() where TModel : ProductViewModel
        {
            var categoryNameAttribute = (CategoryAttribute)Attribute.GetCustomAttribute(typeof(TModel), typeof(CategoryAttribute));

            if (categoryNameAttribute == null)
            {
                throw new ArgumentException($"The model type {typeof(TModel).Name} does not have a Category attribute");
            }

            var query = await this.repository
                .All<Product>()
                .Include(p => p.Manufacturer)
                .Include(p => p.Characteristics)
                .ThenInclude(p => p.CharacteristicName)
                .Where(p => p.Category.Name == categoryNameAttribute.CategoryName)
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
                            Value = pa.Value
                        })
                        .ToList()
                })
                .ToListAsync();

            var products = query.Select(q => CreateModelInstance<TModel>(q)).ToList();

            return products;
        }

        private TModel CreateModelInstance<TModel>(ProductExportModel product)
        {
            var model = Activator.CreateInstance<TModel>();

            foreach (var prop in typeof(TModel).GetProperties())
            {
                var productAttribute = prop.GetCustomAttribute<CharacteristicAttribute>();

                if (productAttribute != null)
                {
                    string name = productAttribute.Name == null
                        ? prop.Name
                        : productAttribute.Name;

                    object characteristicValue = product.Attributes.FirstOrDefault(a => a.Name == name)?.Value;

                    if (characteristicValue == null && prop.Name != "Manufacturer")
                    {
                        continue;
                    }

                    if (prop.Name == "Manufacturer")
                    {
                        characteristicValue = product.Manufacturer;
                    }

                    if (characteristicValue != null && prop.PropertyType != characteristicValue.GetType())
                    {
                        characteristicValue = Convert.ChangeType(characteristicValue, prop.PropertyType);
                    }

                    prop.SetValue(model, characteristicValue);
                }
                else
                {
                    var productProperty = product.GetType().GetProperty(prop.Name);
                    if (productProperty != null)
                    {
                        prop.SetValue(model, productProperty.GetValue(product));
                    }
                }
            }

            return model;
        }

        private async Task<IEnumerable<SearchViewModel>> GetProductsByKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await this.repository.All<Product>()
                    .Include(p => p.Manufacturer)
                    .Select(p => new SearchViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Manufacturer = p.Manufacturer.Name,
                        AddDate = p.AddDate
                    })
                    .ToListAsync();
            }

            var sql = @"SELECT p.* FROM Products p
                LEFT JOIN Manufacturers m ON m.Id = p.ManufacturerId
                WHERE (
                    FREETEXT((p.Name, p.Description, p.Model), @keyword) OR
                    FREETEXT((m.Name), @keyword) OR
                    EXISTS (
                        SELECT 1 FROM Characteristics c 
                        WHERE c.ProductId = p.Id 
                        AND FREETEXT((c.Value), @keyword)
                    )
                )";

            var filtered = await this.repository
                .FromSqlRawAsync<Product>(sql, new SqlParameter("@keyword", keyword));

            var productIds = filtered.Select(p => p.Id).ToList();

            var manufacturers = await this.repository.All<Manufacturer>()
                .Where(m => m.Products.Any(p => productIds.Contains(p.Id)))
                .ToDictionaryAsync(m => m.Id, m => m.Name);

            var result = filtered
                .Select(p => new SearchViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Manufacturer = p.ManufacturerId.HasValue && manufacturers.ContainsKey(p.ManufacturerId.Value) ? manufacturers[p.ManufacturerId.Value] : null,
                    AddDate = p.AddDate
                })
                .ToList();

            return result;
        }

        public async Task<ProductDetailsModel> GetProductDetails(int productId)
        {
            var product = await this.repository
                .AllReadonly<Product>()
                .Include(p => p.Manufacturer)
                .Include(p => p.Characteristics)
                .ThenInclude(p => p.CharacteristicName)
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var model = new ProductDetailsModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                AddDate = product.AddDate,
                Manufacturer = product.Manufacturer?.Name,
                ReferenceNumber = product.ReferenceNumber,
                Description = product.Description,
                Warranty = product.Warranty,
                IsFavorite = false,
                Attributes = product.Characteristics
                        .Select(pa => new ProductAttributeExportModel
                        {
                            Name = pa.CharacteristicName.Name,
                            Value = pa.Value
                        })
                        .ToList()
            };

            return model;
        }

        private IEnumerable<FilterCategoryModel> GetFilterCategories<TModel>(IEnumerable<TModel> products) where TModel : ProductViewModel
        {
            var modelType = typeof(TModel);

            var properties = modelType.GetProperties().Where(p => Attribute.IsDefined(p, typeof(CharacteristicAttribute)));

            var filters = new List<FilterCategoryModel>();

            foreach (var property in properties)
            {
                var values = products
                    .Select(m => property.GetValue(m))
                    .Where(v => v != null)
                    .Select(v => v.ToString())
                    .Distinct()
                    .ToList();

                var filterValues = string.Join(", ", values)
                    .Split(", ")
                    .Select(v => v.Trim())
                    .Distinct();

                var attribute = property.GetCustomAttribute<CharacteristicAttribute>();

                var title = attribute.Name == null ? property.Name : attribute.Name;

                var model = new FilterCategoryModel
                {
                    Name = property.Name,
                    Values = filterValues.First() == "" ? new List<string>() : filterValues.OrderByDescending(p => p),
                    Title = title
                };

                filters.Add(model);
            }

            return filters;
        }

        public async Task<ProductsViewModel<TModel>> GetModel<TModel>() where TModel : ProductViewModel
        {
            var products = await GetProductsAsync<TModel>();
            var model = CreateProductsModel(products);

            return model;
        }

        public async Task<ProductsViewModel<SearchViewModel>> GetSearchModel(string keyword)
        {
            var products = await GetProductsByKeyword(keyword);
            var model = CreateProductsModel(products);

            return model;
        }

        private ProductsViewModel<TModel> CreateProductsModel<TModel>(IEnumerable<TModel> products) where TModel : ProductViewModel
        {
            var filters = GetFilterCategories(products);

            var model = new ProductsViewModel<TModel>
            {
                Filters = filters,
                Products = products
            };

            this.memoryCache.Set("Products", products);

            return model;
        }
    }
}