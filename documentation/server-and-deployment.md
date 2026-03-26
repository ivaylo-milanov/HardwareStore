# Server and deployment

## Applications

The repository ships **two** ASP.NET Core 8.0 web applications:

### 1. HardwareStore.Web.Mvc (main site)

- **Role:** Server-rendered MVC application for customers and administrators.
- **Authentication:** ASP.NET Core **Identity** with the **Customer** entity; cookie-based sessions for the browser.
- **Authorization:** Policy named **`Admin`** (role `Admin`) gates the `/Admin` area.

Typical **development URLs** (from `Properties/launchSettings.json`):

- HTTPS: `https://localhost:7132`
- HTTP: `http://localhost:5170`

### 2. HardwareStore.Web.Api (REST API)

- **Role:** JSON API for programmatic access (mobile clients, integrations, SPA backends).
- **Authentication:** **JWT Bearer** (`Microsoft.AspNetCore.Authentication.JwtBearer`).
- **Documentation:** **Swagger UI** at `/swagger` in Development.

Typical **development URLs**:

- HTTPS: `https://localhost:7133`
- HTTP: `http://localhost:5171`

Startup **fails fast** if the `Jwt` configuration section is missing or if `Jwt:Key` is shorter than 32 characters (see `appsettings` / user secrets).

## Configuration

### Connection string (both apps)

The database is configured via:

```json
"ConnectionStrings": {
  "DefaultConnection": "<your SQL Server connection string>"
}
```

The MVC host resolves it in `ConfigurateDbContext` (`AddDbContextExtension.cs`) and throws if `DefaultConnection` is missing (`ExceptionMessages.DefaultConnectionNotFound`).

**Do not commit production secrets.** Use:

- `appsettings.Development.json` (local only, gitignored if you prefer), or  
- **User secrets** (`dotnet user-secrets`), or  
- Environment variables / Azure Key Vault in production.

### SQL Server provider options

Both applications register EF Core with a shared helper:

- **File:** `HardwareStore.Infrastructure/Data/HardwareStoreSqlServerDbContextOptionsExtensions.cs`
- **Behavior:** `UseSqlServer` with **`EnableRetryOnFailure`** (including transient error **18401** for script-upgrade mode—useful right after SQL Server containers start).

Because of retry execution strategy, **user-initiated transactions** must go through **`IRepository.ExecuteInRetryableTransactionAsync`** (not ad-hoc `BeginTransaction` on the context in application code). The public `BeginTransactionAsync` was removed from `IRepository` for that reason.

### HTTPS

- **Development:** Kestrel URLs from `launchSettings.json`; HTTPS redirection is enabled in the pipeline.
- **Production:** Configure Kestrel/IIS/reverse proxy certificates and `AllowedHosts` as appropriate (`appsettings.json` sets `AllowedHosts` to `*` by default—tighten for production).

### Static files

The MVC app serves `wwwroot` (CSS, JS, images). Admin and catalog views reference Bootstrap and optional CDN assets (e.g. Font Awesome) from the layout.

## Running locally

1. Install [.NET 8 SDK](https://dotnet.microsoft.com/download).
2. Provision **SQL Server** and set `ConnectionStrings:DefaultConnection` for **Mvc** (and **Api** if you run it).
3. Apply EF migrations (from the Infrastructure project, with Mvc or Api as startup):

   ```bash
   dotnet ef database update --project src/Infrastructure/HardwareStore.Infrastructure --startup-project src/Web/HardwareStore.Web.Mvc
   ```

4. For the **API**, configure `Jwt:Key` (≥32 chars), `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpireHours` (see `HardwareStore.Web.Api/appsettings.json` and `JwtOptions`).

5. Run:

   ```bash
   dotnet run --project src/Web/HardwareStore.Web.Mvc
   dotnet run --project src/Web/HardwareStore.Web.Api
   ```

## Migrations endpoint (Development)

The MVC app calls `UseMigrationsEndPoint()` when `ASPNETCORE_ENVIRONMENT` is **Development**, which helps apply pending migrations during local development (see `Program.cs`).
