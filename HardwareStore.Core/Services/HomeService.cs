namespace HardwareStore.Core.Services
{
    using Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class HomeService : IHomeService
    {
        private readonly IRepository repository;

        public HomeService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ProductViewModel>> GetMostBoughtProducts()
            => await this.repository.All<Product>(p => p.ProductsOrders.Count > 3)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToListAsync();

        public async Task<IEnumerable<ProductViewModel>> GetNewProducts()
            => await this.repository.All<Product>()
                .OrderByDescending(p => p.AddDate)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price= p.Price
                })
                .Take(7)
                .ToListAsync();
    }
}
