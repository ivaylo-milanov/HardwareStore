using System.Text;
using HardwareStore.Web.Api.Extensions;
using HardwareStore.Infrastructure.Data;
using HardwareStore.Infrastructure.Models;
using HardwareStore.Web.Api.Options;
using HardwareStore.Web.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var jwtBootstrap = jwtSection.Get<JwtOptions>()
    ?? throw new InvalidOperationException($"Configuration section '{JwtOptions.SectionName}' is missing.");
if (string.IsNullOrWhiteSpace(jwtBootstrap.Key) || jwtBootstrap.Key.Length < 32)
{
    throw new InvalidOperationException("Jwt:Key must be configured and at least 32 characters.");
}

builder.Services.Configure<JwtOptions>(jwtSection);
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "HardwareStore API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddHardwareStoreDbContext(builder.Configuration);
builder.Services.AddHardwareStoreDomainServices();

builder.Services
    .AddIdentity<Customer, IdentityRole>(options => CustomerIdentityOptions.Apply(options))
    .AddEntityFrameworkStores<HardwareStoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBootstrap.Key)),
            ValidateIssuer = true,
            ValidIssuer = jwtBootstrap.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtBootstrap.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

public partial class Program
{
}
