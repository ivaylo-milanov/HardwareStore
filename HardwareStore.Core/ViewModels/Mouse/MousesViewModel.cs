namespace HardwareStore.Core.ViewModels.Mouse
{
    using HardwareStore.Core.ViewModels.Product;

    public class MousesViewModel
    {
        public List<ProductNameValueModel> Prices { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> Sensitivity { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> Manufacturer { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> Interface { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> Sensor { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> Connectivity { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> NumberOfKeys { get; set; } = new List<ProductNameValueModel>();

        public List<ProductNameValueModel> Color { get; set; } = new List<ProductNameValueModel>();

        public IEnumerable<MouseViewModel> Mouses { get; set; } = new List<MouseViewModel>();
    }
}
