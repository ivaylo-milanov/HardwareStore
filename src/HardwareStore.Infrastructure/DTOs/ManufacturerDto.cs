namespace HardwareStore.Infrastructure.DTOs
{
    using Newtonsoft.Json;

    public class ManufacturerDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
