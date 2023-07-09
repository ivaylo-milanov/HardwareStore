namespace HardwareStore.Core.ViewModels.InternalDrive
{
    using HardwareStore.Core.ViewModels.Product;

    public class InternalDriveFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Type { get; set; } = null!;

        public IEnumerable<string> Interface { get; set; } = null!;

        public IEnumerable<string> FormFactor { get; set; } = null!;
    }
}
