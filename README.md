# README

## Overview

This is my project which is a e-commerce hardware platform.

## Features

### Product Catalogue
- **Product Types**: Our vast range of products include:
  - Mouses, Keyboards, Headsets, Monitors, Mouse Pads
  - Processors, Motherboards, RAM, Power Suppliers, Cases
  - Coolers, Video Cards, Internal Drives
  
- **Page Functionalities**:
  - **Filter Products**: Easily narrow down your choices.
  - **Sort Products**: Arrange products as per various criteria.
  - **State Maintenance**: User-friendly interface and state management
  
### Shopping Experience
- **Shopping Cart**:
  - Add products swiftly to your cart.
  - Update the quantity, or remove items as you wish.
  - The items are stored in the session if the customer is not logged in.
  
- **Favorites**:
  - Bookmark products you love.
  - Session storage for visitors, database storage for logged-in users.

### User Profile
- **Logged-In User Features**:
  - Access and modify your profile.
  - View order history.
  - Peruse your favorites list.

- **Ordering**:
  - On successful order placement, you'll be redirected to an order confirmation page.

### Search Functionality
- **Full-Text Search Bar**: Get precise results with indexed search on product Name, Description, Model, and Manufacturer Name.
- **Search Page**: Equipped with product page functionalities for an efficient search experience.

## Database Schema

1. **Product**:
   - Id, Name, Price, Quantity, Description
   - ManufacturerId, Model, Warranty
   - AddDate, CategoryId, ReferenceNumber
   - **Options** (JSON): key/value pairs for filters and product details (replaces former characteristic rows)

2. **Category**:
   - Id, Name
   
3. **Manufacturer**:
   - Id, Name
   
4. **Favorites Mapping Table**:
   - ProductId, CustomerId
   
5. **Shopping Cart Item Mapping Table**:
   - ProductId, CustomerId, Quantity
   
6. **ProductOrder Mapping Table**:
   - ProductId, OrderId, Quantity
   
7. **Order**:
   - Id (GUID type), OrderDate, TotalAmount
   - OrderStatus, PaymentMethod, AdditionalNotes
   - Personal and Address details: FirstName, LastName, Phone, City, Area, Address, CustomerId

8. **Customer (Inherited from IdentityUser)**:
   - FirstName, LastName, City, Area, Address
   
## Tech Stack

- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **User Interface**: JavaScript, CSS, HTML
- **Backend**: C#

## Full-text search indexes

Full-text catalog and indexes are created by the EF migration **`AddFullTextSearchCatalogAndIndexes`** (`20260327000000_AddFullTextSearchCatalogAndIndexes`). Applying migrations with `dotnet ef database update` (see below) runs the equivalent of:

```sql
CREATE FULLTEXT CATALOG product_catalog;

CREATE FULLTEXT INDEX ON Products(Name, Description, Model)
KEY INDEX PK_Products ON product_catalog;

CREATE FULLTEXT INDEX ON Manufacturers(Name)
KEY INDEX PK_Manufacturers ON product_catalog;
```

**Note:** Search in code uses `EF.Functions.Like` in `ProductService`, not `CONTAINS` / `FREETEXT`. The migration runs full-text DDL **only when** `FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1`; on editions without FTS (e.g. some LocalDB setups, or when the engine reports FTS unavailable), that step is skipped and migrations still complete.

## Getting Started

### Connection strings and secrets

- **Do not** put `ConnectionStrings:DefaultConnection` in `appsettings*.json`. It is intentionally omitted from source control.
- Configure it with **.NET User Secrets** on each web project you run (MVC and/or API), for example:

  ```bash
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=127.0.0.1,1433;Database=HardwareStore;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=true" --project src/Web/HardwareStore.Web.Mvc/HardwareStore.Web.Mvc.csproj
  ```

  Use the same pattern for `src/Web/HardwareStore.Web.Api/HardwareStore.Web.Api.csproj` if you run the API.

### SQL password (nothing is stored in the repo)

There is **no default password** in this repository. You choose a strong password that satisfies SQL Server’s policy and use it consistently:

1. **`MSSQL_SA_PASSWORD`** — set in your **shell environment** before Docker Compose (this project does not use a `.env` file in the workflow):

   ```bash
   export MSSQL_SA_PASSWORD='YourStrongPasswordHere'
   docker compose up -d
   ```

2. **User Secrets** — the `Password=` value inside `ConnectionStrings:DefaultConnection` must be the **same** as `MSSQL_SA_PASSWORD` for the `sa` account.

Remember the password yourself (password manager, notes, etc.); it is not committed to git.

**First-time Docker volume:** the `sa` password is fixed when the SQL data volume is first created. If you change `MSSQL_SA_PASSWORD` later but do not recreate the volume, SQL Server still uses the old password.

**If you forgot the password:** check User Secrets (`dotnet user-secrets list --project src/Web/HardwareStore.Web.Mvc`) or your shell history. To reset and pick a new password (this **deletes** container database data):

```bash
docker compose down -v
export MSSQL_SA_PASSWORD='NewPasswordYouWillRemember'
docker compose up -d
```

Then update User Secrets with the new `Password=` value.

### Docker Compose (SQL Server only)

`docker-compose.yml` starts **only** SQL Server in Docker. It does **not** create the `HardwareStore` database or run EF migrations—you do that locally with `dotnet ef` after SQL is accepting connections.

1. Export the SA password (must match `Password=` in your User Secrets connection string):

   ```bash
   export MSSQL_SA_PASSWORD='YourStrongPasswordHere'
   ```

2. From the **repository root**:

   ```bash
   docker compose up -d
   ```

   Wait until the container is healthy (or ~30s on first start). SQL listens on **`127.0.0.1`** and port **`1433`** by default (override host port with `SQL_PORT` if needed).

3. **Create the database and apply all migrations** on your machine (install the tool once if needed: `dotnet tool install --global dotnet-ef`). Run from the repo root with User Secrets already set for the MVC project (so EF can resolve the connection at design time), or pass `--connection` explicitly:

   ```bash
   dotnet ef database update \
     --msbuildprojectextensionspath src/Web/HardwareStore.Web.Mvc/obj \
     --project src/Infrastructure/HardwareStore.Infrastructure/HardwareStore.Infrastructure.csproj \
     --startup-project src/Web/HardwareStore.Web.Mvc/HardwareStore.Web.Mvc.csproj \
     --context HardwareStoreDbContext
   ```

   **`--msbuildprojectextensionspath`** must point at the **startup project’s `obj` folder** (here, MVC) when you run the command from the **repository root**. Otherwise EF may look for `./obj` in the wrong place and report *Unable to retrieve project metadata*.

   This creates the **`HardwareStore`** database if it does not exist and applies every pending migration (schema, seed data, full-text objects, etc.).

   Use the same `--msbuildprojectextensionspath` for other EF commands, for example:

   ```bash
   dotnet ef migrations list \
     --msbuildprojectextensionspath src/Web/HardwareStore.Web.Mvc/obj \
     --project src/Infrastructure/HardwareStore.Infrastructure/HardwareStore.Infrastructure.csproj \
     --startup-project src/Web/HardwareStore.Web.Mvc/HardwareStore.Web.Mvc.csproj \
     --context HardwareStoreDbContext
   ```

4. **Useful Compose commands**
   - Stop containers: `docker compose down`
   - Remove the SQL data volume (wipes all DB files; SA password is re-set only on the next first-time volume create): `docker compose down -v`

On **Apple Silicon (arm64)**, `docker-compose.yml` sets `platform: linux/amd64` for SQL Server so Docker pulls/runs the amd64 image under emulation (required for this official image).

#### Troubleshooting: `SocketException (22): Invalid argument` (macOS)

That error comes from **.NET’s TCP stack** while `Microsoft.Data.SqlClient` is talking to SQL Server (often during the **TLS pre-login** phase). On macOS with **Docker Desktop**, it usually means one of:

- **`localhost` resolves to IPv6 (`::1`)** while the published port is effectively **IPv4** — try **`Server=127.0.0.1,1433`** in your connection string instead of `localhost`.
- **Encryption defaults**: use **`Encrypt=False;TrustServerCertificate=True`** for a typical local Docker SQL instance.
- **SQL not ready** or **container not running** — wait after `docker compose up`, check `docker compose ps`.

#### `Login failed for user 'sa'. Server is in script upgrade mode` (error 18401)

SQL Server is still running **internal startup or upgrade scripts** right after the container (or service) starts. During that window it rejects normal logins, including `sa`. **Wait 30–60 seconds** (longer on first boot or Apple Silicon emulation), watch `docker logs hardwarestore-sql` until the engine reports ready, then run `dotnet ef database update` again.

The app and EF design-time factory use **retry on failure** including error **18401** so brief upgrade windows are retried automatically.

#### `Unable to retrieve project metadata` / `--msbuildprojectextensionspath`

If `dotnet ef` says the project is not SDK-style or suggests **`--msbuildprojectextensionspath`**, you are usually running from the **repo root** while EF’s default `obj` path is relative to that folder, not the startup project.

1. Run **`dotnet restore`** on the solution or MVC project once.
2. Add **`--msbuildprojectextensionspath src/Web/HardwareStore.Web.Mvc/obj`** to every `dotnet ef` command (see step 3 above), or **`cd`** into `src/Web/HardwareStore.Web.Mvc` and run `dotnet ef` with `--project` pointing at Infrastructure (then the default `obj` is correct).
3. If it persists, delete **`bin`** and **`obj`** under the MVC and Infrastructure projects, restore, and retry.

### Other SQL Server instances (no Docker)

Use any reachable SQL Server. Put `ConnectionStrings:DefaultConnection` in User Secrets (host, database name, credentials), then run the same `dotnet ef database update` command as in step 3 above from the repository root.

### Full-text search

Migrations include full-text objects (see [Full-text search indexes](#full-text-search-indexes)). Baseline search still works without them (LIKE-based); apply migrations to install FTS when your SQL Server edition supports it.

### Run the apps

- **MVC site:** run `HardwareStore.Web.Mvc` (e.g. from Visual Studio / Rider / `dotnet run` in that project).
- **API:** run `HardwareStore.Web.Api` if you use it.
