namespace HardwareStore.Core.ViewModels.Home
{
    using HardwareStore.Core.ViewModels.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HomeViewModel
    {
        public IEnumerable<ProductViewModel> MostBoughtProducts { get; set; } = null!;

        public IEnumerable<ProductViewModel> NewestProducts { get; set; } = null!;
    }
}
