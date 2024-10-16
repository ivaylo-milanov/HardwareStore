namespace HardwareStore.Infrastructure.DTOs
{
    using Newtonsoft.Json;

    public class CharacteristicNameDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
