namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Mouse;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class MouseService : IMouseService
    {
        private readonly IRepository repository;

        public MouseService(IRepository repositor)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<MouseViewModel>> GetAllProducts()
            => await this.repository
                .AllReadonly<Product>(b => b.Category.Name == "Mouses")
                .Select(b => new MouseViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    Manufacturer = b.Manufacturer!.Name,
                    AddDate = b.AddDate,
                    Color = b.ProductAttributes.GetAttributeValue(nameof(MouseViewModel.Color)),
                    Connectivity = b.ProductAttributes.GetAttributeValue(nameof(MouseViewModel.Connectivity)),
                    Interface = b.ProductAttributes.GetAttributeValue(nameof(MouseViewModel.Interface)),
                    Sensor = b.ProductAttributes.GetAttributeValue(nameof(MouseViewModel.Sensor)),
                    NumberOfKeys = int.Parse(b.ProductAttributes.GetAttributeValue(nameof(MouseViewModel.NumberOfKeys)))
                })
                .ToListAsync();
    }
}
