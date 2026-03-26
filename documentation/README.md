# HardwareStore — technical documentation

This folder describes how the **HardwareStore** solution is hosted, how the code is organized, and what the application does. It lives **outside** `src/` so it stays separate from build artifacts and project code.

| Document | Contents |
|----------|----------|
| [Server and deployment](server-and-deployment.md) | Web hosts, ports, configuration, secrets, HTTPS, database connectivity |
| [Architecture and codebase](architecture-and-codebase.md) | Solution layout, layers, dependency injection, cross-cutting concerns |
| [Features and functionality](features-and-functionality.md) | Storefront, admin area, REST API, auth, cart, checkout, search, assembly products |
| [Database and data model](database-and-data-model.md) | EF Core context, main entities, migrations, transactions, category assembly slots |

**Solution file:** `src/HardwareStore.sln`

**Primary entry points:**

- **Browser storefront & admin:** `src/Web/HardwareStore.Web.Mvc` (ASP.NET Core MVC + Razor + Identity cookies)
- **REST API:** `src/Web/HardwareStore.Web.Api` (JWT + Swagger)

Both hosts share the same **SQL Server** database via `HardwareStoreDbContext` and the same domain services in `HardwareStore.Core`.
