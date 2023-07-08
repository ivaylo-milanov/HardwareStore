namespace HardwareStore.Core.ViewModels.Mouse
{
    using HardwareStore.Core.ViewModels.Product;

    public class MouseFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Connectivity { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;

        public IEnumerable<string> Interface { get; set; } = null!; 

        public IEnumerable<string> Sensor { get; set; } = null!;

        public IEnumerable<string> ProgrammableButtons { get; set; } = null!;
    }
}
