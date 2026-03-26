# Database and data model

## Technology

- **SQL Server** via **Entity Framework Core 8**
- **Context:** `HardwareStoreDbContext` (`HardwareStore.Infrastructure/Data/HardwareStoreDbContext.cs`)
- **Identity:** `IdentityDbContext<Customer>` — users are **`Customer`** entities with ASP.NET Identity columns and relationships.

## Main entities (DbSets)

| DbSet | Purpose |
|-------|---------|
| **Products** | Sellable items: price, stock, category, manufacturer, `Options` JSON, etc. |
| **Categories** | Product taxonomy; **`Group`** (Hardware / Peripherals); **`AssemblySlot`** (standard BOM slot filter). |
| **Manufacturers** | Optional brand/manufacturer. |
| **Orders** | Customer orders with status, payment, shipping fields. |
| **ProductAssemblyComponent** | **Bill of materials:** parent **assembly product**, child **component product**, **Role** string, **Quantity**, **SortOrder**. |

Related collections are configured on **`Customer`** (favorites, cart items, orders) and **`Product`** (assembly lines, usages in other assemblies) through Fluent API in the `Configurations` folder.

## Category groups and navigation

- **`CategoryGroup`:** `Hardware`, `Peripherals`.
- The storefront **Products** dropdown is built from live **`Categories`** rows (see `CategoriesNavViewComponent`), ordered by name, skipping empty groups.

## Category assembly slots

**`Category.AssemblySlot`** (`CategoryAssemblySlot` enum) tells the **admin assembly editor** and **server validation** which standard PC part type a product in that category satisfies:

| Slot (concept) | Typical use |
|----------------|-------------|
| None | Categories not used for standard slots (e.g. “Prebuilt systems”) or uncategorized assembly use. |
| Cpu, Gpu, Ram, Psu, Motherboard, Case, InternalDrives, Cooler | Align with **`AssemblyRoleKind`** numeric values for filtering (Cooler uses value **9**; there is no “Custom” slot on categories—Custom parts allow any product). |

Predefined hardware categories are **seeded** in migrations; a dedicated migration adds **`AssemblySlot`** and **UPDATE**s known category **names** to the correct slot values.

**Note:** The admin **category** create/edit form may not yet expose **AssemblySlot** in the UI; new categories default to **None** until updated (SQL, migration, or a future admin field).

## Migrations

Migrations live in:

`src/Infrastructure/HardwareStore.Infrastructure/Data/Migrations/`

Notable themes in migration history:

- Initial schema and Identity
- Removal of legacy “characteristics” tables in favor of JSON **Options**
- **CategoryGroup** on categories
- **ProductAssemblyComponent** table and relationships
- Seed predefined hardware **categories**
- **Category.AssemblySlot** column + data fixes for seeded names
- Optional **full-text** catalog/indexes (if present in your branch)

Apply with:

```bash
dotnet ef database update --project src/Infrastructure/HardwareStore.Infrastructure --startup-project src/Web/HardwareStore.Web.Mvc
```

## Transactions and consistency

- **`OrderService`** (and similar flows that need atomic multi-step writes) should use **`IRepository.ExecuteInRetryableTransactionAsync`** so retries do not leave partial state.
- Single **`SaveChangesAsync`** calls are used for simpler CRUD paths.

## Indexes and search

**Default product keyword search** (storefront/API) uses **`EF.Functions.Like`** with escaped `%`/`_`/`[` patterns over product and manufacturer fields in **`ProductService`**.

The solution may also include migrations for **SQL Server full-text** catalogs/indexes (`AddFullTextSearchCatalogAndIndexes` or similar); those are separate from the LIKE-based path unless code is wired to use `FREETEXT`/`CONTAINS`—check the current `ProductService` and migrations in your branch.
