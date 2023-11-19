namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category4")]
    public class MoqProductWIthInvalidCharacteristicName : ProductViewModel
    {
        [Characteristic(Name = "CharacteristicName5")]
        public string CharacteristicName5 { get; set; } = null!;
    }
}
