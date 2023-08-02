namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System.Reflection;

    public class ProductService : IProductService
    {
        private readonly IRepository repository;
        private readonly IMemoryCache memoryCache;
        private readonly IFavoriteService favoriteService;

        public ProductService(IRepository repository, IMemoryCache memoryCache, IFavoriteService favoriteService)
        {
            this.repository = repository;
            this.memoryCache = memoryCache;
            this.favoriteService = favoriteService;
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

        private IEnumerable<TModel> OrderProducts<TModel>(IEnumerable<TModel> products, string order) where TModel : ProductViewModel
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

        private async Task<IEnumerable<TModel>> GetProductsAsync<TModel>() where TModel : ProductViewModel
        {
            var categoryName = typeof(TModel).Name.Replace("ViewModel", string.Empty);

            var query = await this.repository
                .AllReadonly<Product>()
                .Where(p => p.Category.Name == categoryName)
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

        public async Task<IEnumerable<SearchViewModel>> GetProductsByKeyword(string keyword)
        {
            var products = await this.repository
                .AllReadonly<Product>()
                .Include(p => p.Manufacturer)
                .ToListAsync();

            var filtered = products
                .Where(p => ContainsKeyword(p, keyword))
                .Select(p => new SearchViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Manufacturer = p.Manufacturer!.Name,
                    AddDate = p.AddDate
                })
                .ToList();

            return filtered;
        }

        private bool ContainsKeyword(Product product, string keyword)
            => product.Name.ToLower().Contains(keyword) ||
               product.Manufacturer.Name.ToLower().Contains(keyword) ||
               (product.Description != null && product.Description.ToLower().Contains(keyword)) ||
               product.Characteristics.Any(pa => pa.Value.ToLower().Contains(keyword));

        public async Task<ProductDetailsModel> GetProductDetails(int productId)
        {
            var product = await this.repository
                .AllReadonly<Product>()
                .Include(p => p.Manufacturer)
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var isFavorite = await favoriteService.IsFavorite(productId);

            var model = new ProductDetailsModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                AddDate = product.AddDate,
                Manufacturer = product.Manufacturer!.Name,
                ReferenceNumber = product.ReferenceNumber,
                Description = product.Description,
                Warranty = product.Warranty,
                IsFavorite = isFavorite,
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
                var values = products.Select(m => property.GetValue(m).ToString()).Distinct().ToList();

                var filterValues = string.Join(", ", values)
                    .Split(", ")
                    .Select(v => v.Trim())
                    .Distinct();

                var attribute = property.GetCustomAttribute<CharacteristicAttribute>();

                var title = attribute.Name == null ? property.Name : attribute.Name;

                var model = new FilterCategoryModel
                {
                    Name = property.Name,
                    Values = filterValues,
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