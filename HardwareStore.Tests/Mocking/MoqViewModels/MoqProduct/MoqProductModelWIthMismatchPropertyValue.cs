namespace HardwareStore.Tests.Mocking
{
    using Dropbox.Api.Users;
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category4")]
    public class MoqProductModelWIthMismatchPropertyValue : ProductViewModel
    {
        [Characteristic(Name = "CharacteristicName4")]
        public int CharacteristicName4 { get; set; }
    }
}
