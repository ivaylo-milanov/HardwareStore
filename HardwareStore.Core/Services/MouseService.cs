namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Mouse;
    using HardwareStore.Extensions;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class MouseService : IMouseService
    {
        private readonly IRepository repository;

        public MouseService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<MouseViewModel>> GetAllMouses()
            => await this.repository
                .AllReadonly<Product>(b => b.Category.Name == "Mouses")
                .Select(b => new MouseViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    Manufacturer = b.Manufacturer!.Name,
                    Color = b.ProductAttributes.GetAttributeValue("Color"),
                    Connectivity = b.ProductAttributes.GetAttributeValue("Connectivity"),
                    Interface = b.ProductAttributes.GetAttributeValue("Interface"),
                    Sensitivity = int.Parse(b.ProductAttributes.GetAttributeValue("Sensitivity")),
                    Sensor = b.ProductAttributes.GetAttributeValue("Sensor"),
                    NumberOfKeys = int.Parse(b.ProductAttributes.GetAttributeValue("NumberOfKeys")),
                    AddDate = b.AddDate
                })
                .ToListAsync();

        public IEnumerable<MouseViewModel> GetFilteredMouses(IEnumerable<MouseViewModel> mouses, MouseFilterOptions filter)
        {
            if (!filter.Manufacturer.Contains("All"))
            {
                mouses = mouses.Where(m => filter.Manufacturer.Contains(m.Manufacturer));
            }

            if (!filter.Color.Contains("All"))
            {
                mouses = mouses.Where(m => filter.Color.Contains(m.Color));
            }

            if (!filter.Connectivity.Contains("All"))
            {
                mouses = mouses.Where(m => filter.Connectivity.Contains(m.Connectivity));
            }

            if (!filter.Interface.Contains("All"))
            {
                mouses = mouses.Where(m => filter.Interface.Contains(m.Interface));
            }

            if (!filter.Sensor.Contains("All"))
            {
                mouses = mouses.Where(m => filter.Sensor.Contains(m.Sensor));
            }

            if (!filter.NumberOfKeys.Contains("All"))
            {
                mouses = mouses.Where(m => filter.NumberOfKeys.Contains(m.NumberOfKeys.ToString()));
            }

            if (!filter.Sensitivity.Contains("All"))
            {
                mouses = GetFilteredValuesBySensitivity(mouses, filter.Sensitivity);
            }

            if (!filter.Price.Contains("All"))
            {
                mouses = GetFilteredValuesByPrice(mouses, filter.Price);
            }

            if (!filter.Order.Contains("Default"))
            {
                mouses = mouses.OrderProducts(filter.Order.First());
            }

            return mouses;
        }

        private IEnumerable<MouseViewModel> GetFilteredValuesBySensitivity(IEnumerable<MouseViewModel> mouses, IEnumerable<string> sensitivityValues)
        {
            var filteredMouses = new List<MouseViewModel>();

            foreach (var sensitivity in sensitivityValues)
            {
                int sens = int.Parse(sensitivity);

                switch (sens)
                {
                    case 1:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity <= 800));
                        break;
                    case 2:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity > 800 && m.Sensitivity <= 1200));
                        break;
                    case 3:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity > 1200 && m.Sensitivity <= 2000));
                        break;
                    case 4:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity > 2000 && m.Sensitivity <= 3000));
                        break;
                    case 5:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity > 3000 && m.Sensitivity <= 4000));
                        break;
                    case 6:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity > 4000 && m.Sensitivity < 10000));
                        break;
                    case 7:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity >= 10000));
                        break;
                }
            }

            return filteredMouses;
        }

        private IEnumerable<MouseViewModel> GetFilteredValuesByPrice(IEnumerable<MouseViewModel> mouses, IEnumerable<string> priceValues)
        {
            var filteredMouses = new List<MouseViewModel>();

            foreach (var price in priceValues)
            {
                int priceValue = int.Parse(price);

                switch (priceValue)
                {
                    case 1:
                        filteredMouses.AddRange(mouses.Where(m => m.Price <= 100));
                        break;
                    case 2:
                        filteredMouses.AddRange(mouses.Where(m => m.Price > 100 && m.Price <= 200));
                        break;
                    case 3:
                        filteredMouses.AddRange(mouses.Where(m => m.Price > 200 && m.Price <= 300));
                        break;
                    case 4:
                        filteredMouses.AddRange(mouses.Where(m => m.Price > 300 && m.Price <= 400));
                        break;
                    case 5:
                        filteredMouses.AddRange(mouses.Where(m => m.Sensitivity > 400));
                        break;
                }
            }

            return filteredMouses;
        }
    }
}
