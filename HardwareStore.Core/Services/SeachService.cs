namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Search;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SeachService : ISearchService
    {
        private readonly IRepository repository;

        public SeachService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<SearchViewModel>> GetProductsByKeyword(string keyword)
            => await this.repository.AllReadonly<Product>(p => ContainsKeyword(p, keyword.ToLower()))
                .Select(p => new SearchViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    AddDate = p.AddDate,
                    Manufacturer = p.Manufacturer!.Name
                })
                .ToListAsync();

        private bool ContainsKeyword(Product product, string keyword)
            => product.Name.ToLower().Contains(keyword) ||
               product.Price.ToString().ToLower().Contains(keyword) ||
               product.Manufacturer.Name.ToLower().Contains(keyword) ||
               (product.Description != null && product.Description.ToLower().Contains(keyword)) ||
               (product.Model != null && product.Description.ToLower().Contains(keyword)) ||
               product.ProductAttributes.Any(pa => pa.Value.ToLower().Contains(keyword));
    }
}
