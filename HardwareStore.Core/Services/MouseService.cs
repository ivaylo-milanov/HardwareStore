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
        private readonly IDropboxService dropboxService;

        public MouseService(IRepository repository, IDropboxService dropboxService)
        {
            this.repository = repository;
            this.dropboxService = dropboxService;
        }

        public async Task<IEnumerable<MouseViewModel>> GetAllProducts()
            => await this.repository
                .AllReadonly<Product>(b => b.Category.Name == "Mouse")
                .Select(b => new MouseViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    Manufacturer = b.Manufacturer!.Name,
                    AddDate = b.AddDate,
                    //ImageUrl = dropboxService.GetProductFirstImageAsync(b.Id),
                    Color = b.ProductAttributes.GetAttributeValue("Color"),
                    Connectivity = b.ProductAttributes.GetAttributeValue("Connectivity"),
                    Interface = b.ProductAttributes.GetAttributeValue("Interface"),
                    Sensor = b.ProductAttributes.GetAttributeValue("Sensor"),
                    NumberOfKeys = int.Parse(b.ProductAttributes.GetAttributeValue("NumberOfKeys"))
                })
                .ToListAsync();
    }
}
