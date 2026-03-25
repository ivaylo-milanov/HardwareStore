namespace HardwareStore.Core.ViewModels
{
    /// <summary>Root shape of wwwroot/data/categories-nav.json.</summary>
    public class CategoriesNavModel
    {
        public NavCategoryItem[] Peripherals { get; set; } = null!;

        public NavCategoryItem[] Hardware { get; set; } = null!;
    }
}
