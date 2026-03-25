namespace HardwareStore.Web.Api.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Web.Api.Options;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions options;
        private readonly TimeSpan lifetime;

        public JwtTokenService(IOptions<JwtOptions> optionsAccessor)
        {
            this.options = optionsAccessor.Value;
            if (string.IsNullOrWhiteSpace(this.options.Key) || this.options.Key.Length < 32)
            {
                throw new InvalidOperationException("Jwt:Key must be at least 32 characters.");
            }

            this.lifetime = TimeSpan.FromHours(Math.Max(1, this.options.ExpireHours));
        }

        public int ExpiresInSeconds => (int)this.lifetime.TotalSeconds;

        public string CreateToken(Customer user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Name, user.UserName ?? user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: this.options.Issuer,
                audience: this.options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(this.lifetime),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
