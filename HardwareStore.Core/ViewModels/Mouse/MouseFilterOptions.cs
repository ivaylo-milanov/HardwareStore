namespace HardwareStore.Core.ViewModels.Mouse
{
    public class MouseFilterOptions
    {
        public List<string> Manufacturer { get; set; } = new List<string>();

        public List<string> Price { get; set; } = new List<string>();

        public List<string> Connectivity { get; set; } = new List<string>();

        public List<string> Color { get; set; } = new List<string>();

        public List<string> Interface { get; set; } = new List<string>();

        public List<string> Sensitivity { get; set; } = new List<string>();

        public List<string> Sensor { get; set; } = new List<string>();

        public List<string> NumberOfKeys { get; set; } = new List<string>();

        public List<string> Order { get; set; } = new List<string>();
    }
}
