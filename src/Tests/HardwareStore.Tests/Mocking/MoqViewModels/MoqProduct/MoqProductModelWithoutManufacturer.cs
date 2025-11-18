namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category3")]
    public class MoqProductModelWithoutManufacturer : ProductViewModel
    {
        [Characteristic(Name = "CharacteristicName2")]
        public string CharacteristicName2 { get; set; } = null!;
    }
}
