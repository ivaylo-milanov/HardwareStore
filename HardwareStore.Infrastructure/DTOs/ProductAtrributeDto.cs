namespace HardwareStore.Infrastructure.DTOs
{
    using Newtonsoft.Json;

    public class ProductAtrributeDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("value")]
        public string Value { get; set; } = null!;
    }
}
