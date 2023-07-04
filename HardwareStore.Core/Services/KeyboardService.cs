namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Keyboard;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class KeyboardService : IKeyboardService
    {
        private readonly IRepository repository;

        public KeyboardService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<KeyboardViewModel>> GetAllProducts()
            => await this.repository.AllReadonly<Product>(k => k.Category.Name == "Keyboard")
                .Select(k => new KeyboardViewModel
                {
                    Id = k.Id,
                    Name = k.Name,
                    Price = k.Price,
                    AddDate = k.AddDate,
                    Manufacturer = k.Manufacturer!.Name,
                    Connectivity = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Connectivity)),
                    Color = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Color)),
                    Interface = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Interface)),
                    Type = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Type)),
                    Form = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Form)),
                    Backlight = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Backlight)),
                    Cyrillicization = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Connectivity)),
                    ButtonType = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.ButtonType)),
                    MacroButtons = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.MacroButtons)),
                    MultiMediaButtons = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.MultiMediaButtons)),
                    Switch = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Switch)),
                    Layout = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.Layout)),
                    HotSwappable = k.ProductAttributes.GetAttributeValue(nameof(KeyboardViewModel.HotSwappable))
                })
                .ToListAsync();
    }
}