namespace HardwareStore.Core.ViewModels.VideoCard
{
    using HardwareStore.Core.ViewModels.Product;

    public class VideoCardFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Series { get; set; } = null!;

        public IEnumerable<string> Memory { get; set; } = null!;

        public IEnumerable<string> Outputs { get; set; } = null!;

        public IEnumerable<string> MemoryType { get; set; } = null!;
    }
}
