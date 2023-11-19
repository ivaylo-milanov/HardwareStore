namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Category4")]
    public class MoqProductModelWithCharacteristicNameCollision : ProductViewModel
    {
        [Characteristic(Name = "Id")]
        public int MockId { get; set; }
    }
}
