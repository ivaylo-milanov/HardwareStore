namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category1")]
    public class MoqProductModelWithProperCategory : ProductViewModel
    {
        [Characteristic(Name = "CharacteristicName1")]
        public string CharacteristicName1 { get; set; }

        [Characteristic(Name = "CharacteristicName2")]
        public string CharacteristicName2 { get; set; }

        [Characteristic(Name = "CharacteristicName3")]
        public string CharacteristicName3 { get; set; }
    }
}
