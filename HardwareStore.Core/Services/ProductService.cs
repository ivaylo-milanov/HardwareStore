namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class ProductService : IProductService
    {
        private readonly IRepository repository;

        public ProductService(IRepository repository)
        {
            this.repository = repository;
        }

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

        public async Task<IEnumerable<TModel>> GetProductsAsync<TModel>() where TModel : ProductViewModel
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

        public async Task<IEnumerable<ProductViewModel>> GetProductsByKeyword(string keyword)
        {
            var products = await this.repository
                .AllReadonly<Product>()
                .ToListAsync();

            var filtered = products
                .Where(p => ContainsKeyword(p, keyword))
                .Select(p => new ProductViewModel
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
               product.Price.ToString().ToLower().Contains(keyword) ||
               product.Manufacturer.Name.ToLower().Contains(keyword) ||
               (product.Description != null && product.Description.ToLower().Contains(keyword)) ||
               (product.Model != null && product.Description.ToLower().Contains(keyword)) ||
               product.Characteristics.Any(pa => pa.Value.ToLower().Contains(keyword));

        public async Task<ProductDetailsModel> GetProductDetails(int id)
        {
            var product = await this.repository
                .AllReadonly<Product>()
                .Include(p => p.Manufacturer)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

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
    }
}