# HardwareStore — technical documentation

This folder describes how the **HardwareStore** solution is hosted, how the code is organized, and what the application does. It lives **outside** `src/` so it stays separate from build artifacts and project code.

| Document | Contents |
|----------|----------|
| [Server and deployment](server-and-deployment.md) | Web hosts, ports, configuration, secrets, HTTPS, database connectivity, **Docker Compose** for SQL Server |
| [Architecture and codebase](architecture-and-codebase.md) | Solution layout, layers, dependency injection, cross-cutting concerns, links to deeper inventories |
| [Features and functionality](features-and-functionality.md) | Feature-by-feature behavior (MVC + API), flows, links to source and views |
| [Database and data model](database-and-data-model.md) | EF Core context, ER overview, entities, migrations, transactions, search vs FTS |
| [**Solution inventory**](solution-inventory.md) | **Verbose per-file catalog** of `src/` (excluding redundant EF Designer files) + migration summaries |
| [**API endpoints**](api-endpoints.md) | **REST** reference: method, route, auth, request/response shapes |
| [**UI and views**](ui-views.md) | **Every Razor** view/partial: model, controller, catalog pipeline diagram |

**Solution file:** `src/HardwareStore.sln`

**Primary entry points:**

- **Browser storefront & admin:** `src/Web/HardwareStore.Web.Mvc` (ASP.NET Core MVC + Razor + Identity cookies)
- **REST API:** `src/Web/HardwareStore.Web.Api` (JWT + Swagger)

Both hosts share the same **SQL Server** database via `HardwareStoreDbContext` and the same domain services in `HardwareStore.Core`.
