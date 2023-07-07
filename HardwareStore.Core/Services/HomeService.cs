namespace HardwareStore.Core.Services
{
    using Contracts;
    using HardwareStore.Core.ViewModels.Home;
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

        public async Task<IEnumerable<MostBoughtProductViewModel>> GetMostBoughtProducts()
            => await this.repository.All<Product>(/*p => p.ProductsOrders.Count > 3*/)
                .Select(p => new MostBoughtProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                
                .ToListAsync();

        public async Task<IEnumerable<NewProductViewModel>> GetNewProducts()
            => await this.repository.All<Product>()
                .OrderByDescending(p => p.AddDate)
                .Select(p => new NewProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price= p.Price
                })
                .Take(7)
                .ToListAsync();
    }
}
