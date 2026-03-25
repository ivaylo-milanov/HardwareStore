namespace HardwareStore.Web.Api.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public int ExpireHours { get; set; } = 8;
    }
}
