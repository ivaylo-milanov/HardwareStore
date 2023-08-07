namespace HardwareStore.Tests.Mocking.MoqViewModels.MoqProduct
{
    using HardwareStore.Core.ViewModels.Product;

    public class MoqProductFilterOptionsCategory1 : ProductFilterOptions
    {
        public IEnumerable<string> CharacteristicName1 { get; set; } = null!;

        public IEnumerable<string> CharacteristicName2 { get; set; } = null!;

        public IEnumerable<string> CharacteristicName3 { get; set; } = null!;
    }
}
