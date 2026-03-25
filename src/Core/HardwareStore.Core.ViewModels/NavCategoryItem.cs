namespace HardwareStore.Core.ViewModels
{
    using System.Text.Json.Serialization;

    /// <summary>One row from categories navigation JSON (not the domain Category entity).</summary>
    public class NavCategoryItem
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
