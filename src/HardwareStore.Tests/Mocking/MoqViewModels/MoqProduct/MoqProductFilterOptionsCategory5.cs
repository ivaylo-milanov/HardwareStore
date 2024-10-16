namespace HardwareStore.Tests.Mocking.MoqViewModels.MoqProduct
{
    using HardwareStore.Core.ViewModels.Product;

    public class MoqProductFilterOptionsCategory5 : ProductFilterOptions
    {
        public IEnumerable<string> CharacteristicName2 { get; set; } = null!;

        public IEnumerable<string> CharacteristicName4 { get; set; } = null!;
    }
}
