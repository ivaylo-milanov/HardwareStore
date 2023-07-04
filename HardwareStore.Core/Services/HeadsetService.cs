namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Headset;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class HeadsetService : IHeadsetService
    {
        private readonly IRepository repository;

        public HeadsetService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<HeadsetViewModel>> GetAllProducts()
            => await this.repository.AllReadonly<Product>(p => p.Category.Name == "Headset")
                .Select(p => new HeadsetViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    AddDate = p.AddDate,
                    Price = p.Price,
                    Manufacturer = p.Manufacturer!.Name,
                    Form = p.ProductAttributes.GetAttributeValue(nameof(HeadsetViewModel.Form)),
                    Interface = p.ProductAttributes.GetAttributeValue(nameof(HeadsetViewModel.Interface)),
                    NoiseIsolation = p.ProductAttributes.GetAttributeValue(nameof(HeadsetViewModel.NoiseIsolation)),
                    Type = p.ProductAttributes.GetAttributeValue(nameof(HeadsetViewModel.Type)),
                    Compatibility = p.ProductAttributes.GetAttributeValue(nameof(HeadsetViewModel.Compatibility)),
                    Color = p.ProductAttributes.GetAttributeValue(nameof(HeadsetViewModel.Color))
                })
                .ToListAsync();
    }
}
