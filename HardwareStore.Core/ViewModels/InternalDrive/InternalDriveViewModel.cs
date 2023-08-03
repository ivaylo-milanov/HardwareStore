namespace HardwareStore.Core.ViewModels.InternalDrive
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Internal Drive")]
    public class InternalDriveViewModel : ProductViewModel
    {
        [Characteristic]
        public string Type { get; set; } = null!;

        [Characteristic]
        public string Interface { get; set; } = null!;

        [Characteristic("Form Factor")]
        public string FormFactor { get; set; } = null!;
    }
}
