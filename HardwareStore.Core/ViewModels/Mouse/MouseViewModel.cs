namespace HardwareStore.Core.ViewModels.Mouse
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Mouse")]
    public class MouseViewModel : ProductViewModel
    {
        [Characteristic]
        public string Connectivity { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!;

        [Characteristic]
        public string Interface { get; set; } = null!;

        [Characteristic]
        public string Sensor { get; set; } = null!;

        [Characteristic("Programmable buttons")]
        public string ProgrammableButtons { get; set; } = null!;
    }
}
