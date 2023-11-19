namespace HardwareStore.Core.ViewModels.Case
{
    using HardwareStore.Core.ViewModels.Product;

    public class CaseFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> FormFactor { get; set; } = null!;

        public IEnumerable<string> Format { get; set; } = null!;

        public IEnumerable<string> FansIncluded { get; set; } = null!;

        public IEnumerable<string> FrontPanel { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;

        public IEnumerable<string> SupportedFansFront { get; set; } = null!;

        public IEnumerable<string> SupportedFansBelow { get; set; } = null!;

        public IEnumerable<string> SupportedFansRear { get; set; } = null!;

        public IEnumerable<string> SupportedFansTop { get; set; } = null!;

        public IEnumerable<string> SupportedFansSides { get; set; } = null!;

        public IEnumerable<string> SupportedWaterCoolersFront { get; set; } = null!;

        public IEnumerable<string> SupportedWaterCoolersBelow { get; set; } = null!;

        public IEnumerable<string> SupportedWaterCoolersRear { get; set; } = null!;

        public IEnumerable<string> SupportedWaterCoolersTop { get; set; } = null!;

        public IEnumerable<string> SupportedWaterCoolersSides { get; set; } = null!;

        public IEnumerable<string> FrontMeshPanel { get; set; } = null!;
    }
}
