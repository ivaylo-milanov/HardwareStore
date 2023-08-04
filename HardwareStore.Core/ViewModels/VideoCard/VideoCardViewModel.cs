namespace HardwareStore.Core.ViewModels.VideoCard
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Video Card")]
    public class VideoCardViewModel : ProductViewModel
    {
        [Characteristic]
        public string Series { get; set; } = null!;

        [Characteristic]
        public string Memory { get; set; } = null!;

        [Characteristic]
        public string Outputs { get; set; } = null!;

        [Characteristic("Memory type")]
        public string MemoryType { get; set; } = null!;
    }
}
