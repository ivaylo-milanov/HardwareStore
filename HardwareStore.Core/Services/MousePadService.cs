namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.MousePad;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MousePadService : IMousePadService
    {
        private readonly IRepository repository;

        public MousePadService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<MousePadViewModel>> GetAllProducts()
            => await this.repository.AllReadonly<Product>(p => p.Category.Name == "MousePad")
                .Select(mp => new MousePadViewModel
                {
                    Id = mp.Id,
                    Name = mp.Name,
                    Price = mp.Price,
                    AddDate = mp.AddDate,
                    Manufacturer = mp.Manufacturer!.Name,
                    Surface = mp.ProductAttributes.GetAttributeValue("Surface"),
                    Cover = mp.ProductAttributes.GetAttributeValue("Cover"),
                    Backlight = mp.ProductAttributes.GetAttributeValue("Backlight"),
                    Color = mp.ProductAttributes.GetAttributeValue("Color")
                })
                .ToListAsync();
    }
}
