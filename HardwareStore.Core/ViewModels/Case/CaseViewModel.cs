namespace HardwareStore.Core.ViewModels.Case
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    public class CaseViewModel : ProductViewModel
    {
        [Characteristic("Form Factor")]
        public string FormFactor { get; set; } = null!;

        [Characteristic]
        public string Format { get; set; } = null!;

        [Characteristic("Fans included")]
        public string FansIncluded { get; set; } = null!;

        [Characteristic("Front panel")]
        public string FrontPanel { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!;

        [Characteristic("Supported fans in the front")]
        public string SupportedFansFront { get; set; } = null!;

        [Characteristic("Supported fans below")]
        public string SupportedFansBelow { get; set; } = null!;

        [Characteristic("Supported rear fans")]
        public string SupportedFansRear { get; set; } = null!;

        [Characteristic("Supported fans on top")]
        public string SupportedFansTop { get; set; } = null!;

        [Characteristic("Supported fans on the sides")]
        public string SupportedFansSides { get; set; } = null!;

        [Characteristic("Supported water coolers in the front")]
        public string SupportedWaterCoolersFront { get; set; } = null!;

        [Characteristic("Supported water coolers below")]
        public string SupportedWaterCoolersBelow { get; set; } = null!;

        [Characteristic("Supported rear water coolers")]
        public string SupportedWaterCoolersRear { get; set; } = null!;

        [Characteristic("Supported water cooler on top")]
        public string SupportedWaterCoolersTop { get; set; } = null!;

        [Characteristic("Supported water cooler on the sides")]
        public string SupportedWaterCoolersSides { get; set; } = null!;

        [Characteristic("Front Mesh panel")]
        public string FrontMeshPanel { get; set; } = null!;
    }
}
