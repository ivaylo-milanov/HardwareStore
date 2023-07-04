namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Monitor;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MonitorService : IMonitorService
    {
        private readonly IRepository repository;

        public MonitorService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<MonitorViewModel>> GetAllProducts()
            => await this.repository.AllReadonly<Product>(p => p.Category.Name == "Monitor")
                .Select(m => new MonitorViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    AddDate = m.AddDate,
                    Manufacturer = m.Manufacturer!.Name,
                    Resolution = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.Resolution)),
                    Matrix = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.Matrix)),
                    Ports = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.Ports)),
                    Technology = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.Technology)),
                    StandAdjustment = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.StandAdjustment)),
                    TouchScreen = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.TouchScreen)),
                    VESA = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.VESA)),
                    Color = m.ProductAttributes.GetAttributeValue(nameof(MonitorViewModel.Color))
                })
                .ToListAsync();
    }
}
