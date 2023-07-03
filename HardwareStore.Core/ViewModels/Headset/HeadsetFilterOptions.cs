namespace HardwareStore.Core.ViewModels.Headset
{
    using HardwareStore.Core.ViewModels.Product;

    public class HeadsetFilterOptions : ProductFilterOptions
    {
        public List<string> Form { get; set; } = new List<string>();

        public List<string> Interface { get; set; } = new List<string>();

        public List<string> NoiseIsolation { get; set; } = new List<string>();

        public List<string> Type { get; set; } = new List<string>();

        public List<string> Compatibility { get; set; } = new List<string>();

        public List<string> Color { get; set; } = new List<string>(); 
    }
}
