namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class DetailsService : IDetailsService
    {
        private readonly IRepository repository;

        public DetailsService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ProductDetailsModel> GetProductDetails(int productId)
        {
            var product = await this.repository
                .All<Product>()
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

        public async Task<bool> IsProductInDbFavorites(string customerId, int productId)
        {
            var customer = await this.repository.FindAsync<Customer>(customerId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            return await this.repository.AnyAsync<Favorite>(f => f.CustomerId == customerId && f.ProductId == productId);
        }

        public async Task<bool> IsProductInSessionFavorites(ICollection<int> sessionFavorites, int productId)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            return sessionFavorites != null && sessionFavorites.Contains(productId);
        }
    }
}
