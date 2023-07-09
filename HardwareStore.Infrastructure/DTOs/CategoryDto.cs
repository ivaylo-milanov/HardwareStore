namespace HardwareStore.Infrastructure.DTOs
{
    using Newtonsoft.Json;

    public class CategoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
