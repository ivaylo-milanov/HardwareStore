namespace HardwareStore.Core.ViewModels.VideoCard
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    public class VideoCardViewModel : ProductViewModel
    {
        [Characteristic]
        public string Series { get; set; } = null!;

        [Characteristic]
        public int Memory { get; set; }

        [Characteristic]
        public string Outputs { get; set; } = null!;

        [Characteristic("Type of memory")]
        public string MemoryType { get; set; } = null!;
    }
}
