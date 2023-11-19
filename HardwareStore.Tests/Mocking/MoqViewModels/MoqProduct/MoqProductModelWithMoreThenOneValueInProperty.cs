namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category5")]
    public class MoqProductModelWithMoreThenOneValueInProperty : ProductViewModel
    {
        [Characteristic(Name = "CharacteristicName2")]
        public string CharacteristicName2 { get; set; } = null!;

        [Characteristic(Name = "CharacteristicName4")]
        public string CharacteristicName4 { get; set; } = null!;
    }
}
