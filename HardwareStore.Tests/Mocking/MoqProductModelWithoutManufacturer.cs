namespace HardwareStore.Tests.Mocking
{
    using Dropbox.Api.Users;
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category3")]
    public class MoqProductModelWithoutManufacturer : ProductViewModel
    {
        [Characteristic(Name = "CharacteristicName2")]
        public string CharacteristicName2 { get; set; } = null!;
    }
}
