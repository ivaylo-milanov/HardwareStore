namespace HardwareStore.Core.ViewModels.Mouse
{
    using HardwareStore.Core.ViewModels.Product;

    public class MouseFilterOptions : ProductFilterOptions
    {
        public List<string> Connectivity { get; set; } = new List<string>();

        public List<string> Color { get; set; } = new List<string>();

        public List<string> Interface { get; set; } = new List<string>();

        public List<string> Sensor { get; set; } = new List<string>();

        public List<string> NumberOfKeys { get; set; } = new List<string>();
    }
}
