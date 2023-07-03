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
                    Connectivity = k.ProductAttributes.GetAttributeValue("Connectivity"),
                    Color = k.ProductAttributes.GetAttributeValue("Color"),
                    Interface = k.ProductAttributes.GetAttributeValue("Interface"),
                    Type = k.ProductAttributes.GetAttributeValue("Type"),
                    Form = k.ProductAttributes.GetAttributeValue("Form"),
                    Backlight = k.ProductAttributes.GetAttributeValue("Backlight"),
                    Cyrillicization = k.ProductAttributes.GetAttributeValue("Cyrillicization"),
                    ButtonType = k.ProductAttributes.GetAttributeValue("ButtonType"),
                    MacroButtons = k.ProductAttributes.GetAttributeValue("MacroButtons"),
                    MultiMediaButtons = k.ProductAttributes.GetAttributeValue("MultiMediaNuttons"),
                    Switch = k.ProductAttributes.GetAttributeValue("Switch"),
                    Layout = k.ProductAttributes.GetAttributeValue("Layout"),
                    HotSwappable = k.ProductAttributes.GetAttributeValue("HotSwappable")
                })
                .ToListAsync();

        //public IEnumerable<KeyboardViewModel> GetFilteredProducts(IEnumerable<KeyboardViewModel> products, KeyboardFilterOptions filter)
        //{
        //    if (filter.Manufacturer.Count > 0)
        //    {
        //        products = products.Where(p => filter.Manufacturer.Contains(p.Manufacturer));
        //    }

        //    if (filter.Connectivity.Count > 0)
        //    {
        //        products = products.Where(p => filter.Connectivity.Contains(p.Connectivity));
        //    }

        //    if (filter.Color.Count > 0)
        //    {
        //        products = products.Where(p => filter.Color.Contains(p.Color));
        //    }

        //    if (filter.Type.Count > 0)
        //    {
        //        products = products.Where(p => filter.Type.Contains(p.Type));
        //    }

        //    if (filter.Form.Count > 0)
        //    {
        //        products = products.Where(p => filter.Form.Contains(p.Form));
        //    }

        //    if (filter.Backlight.Count > 0)
        //    {
        //        products = products.Where(p => filter.Backlight.Contains(p.Backlight));
        //    }

        //    if (filter.Cyrillicization.Count > 0)
        //    {
        //        products = products.Where(p => filter.Cyrillicization.Contains(p.Cyrillicization));
        //    }

        //    if (filter.ButtonType.Count > 0)
        //    {
        //        products = products.Where(p => filter.ButtonType.Contains(p.ButtonType));
        //    }

        //    if (filter.MacroButtons.Count > 0)
        //    {
        //        products = products.Where(p => filter.MacroButtons.Contains(p.MacroButtons));
        //    }

        //    if (filter.MultiMediaButtons.Count > 0)
        //    {
        //        products = products.Where(p => filter.MultiMediaButtons.Contains(p.MultiMediaButtons));
        //    }

        //    if (filter.Switch.Count > 0)
        //    {
        //        products = products.Where(p => filter.Switch.Contains(p.Switch));
        //    }

        //    if (filter.Layout.Count > 0)
        //    {
        //        products = products.Where(p => filter.Layout.Contains(p.Layout));
        //    }

        //    if (filter.HotSwappable.Count > 0)
        //    {
        //        products = products.Where(p => filter.HotSwappable.Contains(p.HotSwappable));
        //    }

        //    if (filter.Order != null)
        //    {
        //        products = products.OrderProducts(filter.Order);
        //    }

        //    return products;
        //}
    }
}