namespace HardwareStore.Core.ViewModels.Headset
{
    using HardwareStore.Core.ViewModels.Product;

    public class HeadsetFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Form { get; set; } = null!;

        public IEnumerable<string> Interface { get; set; } = null!;

        public IEnumerable<string> NoiseIsolation { get; set; } = null!;

        public IEnumerable<string> Type { get; set; } = null!;

        public IEnumerable<string> Compatibility { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!; 
    }
}
