# Features and functionality

This document maps **user-visible behavior** to the main **controllers and services**. Paths are relative to the MVC project unless noted.

## Storefront (anonymous and authenticated customers)

### Home

- **`HomeController`** — landing page.

### Product catalog and details

- **`ProductController`** — category browsing, product listing, product details.
- **`ProductService`** — loads product data, options (JSON), and **assembly components** (BOM) for bundle/PC products via `ProductAssemblyComponent` rows.

Products support a flexible **`Options`** JSON object for specifications (validated as JSON object on admin save).

### Search

- **`SearchController`** (MVC) and **`SearchController`** (API) — keyword search delegated to **`ProductService`**, which matches the term with **`EF.Functions.Like`** against name, description, model, and manufacturer name (see `LoadSearchByKeywordAsync` in `ProductService.cs`). A separate migration may add full-text catalog objects for other scenarios; the primary catalog search path is LIKE-based.

### Favorites

- **`FavoriteController`** + **`IFavoriteService`** — persist favorite products per logged-in user.

### Shopping cart

- **`CartController`** + **`IShoppingCartService`** — add/update/remove lines; cart is stored per **Customer** in the database (not session-only).

### Checkout and orders

- **`CheckoutController`** — checkout flow; typically requires sign-in (`[Authorize]`).
- **`OrdersController`** — order history for the current user.
- **`IOrderService`** — creates orders from cart; uses **`IRepository.ExecuteInRetryableTransactionAsync`** so order creation is atomic under SQL Server retry policy.

### Profile

- **`ProfileController`** — account/profile management views.

### Authentication (MVC)

- **`UserController`** — login/register flows (along with Identity Razor pages under `Areas/Identity` if enabled).

---

## Administration (`/Admin` area)

All admin controllers inherit from **`AdminControllerBase`**, which enforces the **`Admin`** authorization policy.

| Controller | Purpose |
|------------|---------|
| **`ProductsController`** | CRUD products, JSON **Options**, **assembly components** (part type, component product, quantity). Validates that component products’ **category assembly slot** matches the selected standard role (CPU, GPU, …) when not Custom. |
| **`CategoriesController`** | CRUD categories; **Name** and **Group** (Hardware / Peripherals). *Assembly slot* is stored on `Category` but may require DB/migration updates or future admin fields—see [Database](database-and-data-model.md). |
| **`ManufacturersController`** | CRUD manufacturers. |
| **`CustomersController`** | Manage customer accounts (admin-facing). |
| **`HomeController`** | Admin home/dashboard entry. |

### Assembly (bundle / PC) editor

- Admin product forms include **`_AssemblyComponentsPartial`**.
- **Standard roles** map to persisted strings on `ProductAssemblyComponent.Role` (e.g. `CPU`, `GPU`, `Motherboard`, `InternalDrives`, `Cooler`, `Custom` label).
- The UI loads a JSON catalog `{ id, label, slot }` where **`slot`** is the integer value of **`Category.AssemblySlot`**. JavaScript filters options per row by matching **`slot`** to the selected **`AssemblyRoleKind`** enum value (Custom / None show all products).
- Server-side validation mirrors the same rules via **`AssemblyRoleMapping`** in **Core.ViewModels**.

---

## REST API (`HardwareStore.Web.Api`)

JWT-protected endpoints mirror many storefront operations:

| Area | Controller (examples) |
|------|------------------------|
| Auth | **`AuthController`** — token issuance |
| Catalog | **`CategoryCatalogController`**, **`ProductDetailsController`** |
| Cart | **`CartController`** |
| Checkout | **`CheckoutController`** |
| Orders | **`OrdersController`** |
| Favorites | **`FavoritesController`** |
| Profile | **`ProfileController`** |
| Search | **`SearchController`** |

Swagger is configured with a **Bearer** security scheme for testing authenticated calls.

---

## Cross-cutting rules

- **Antiforgery:** Enabled by default on MVC controllers.
- **Roles:** `Admin` role seeded via migrations (see migration `SeedAdminUserAndRole` in Infrastructure).
- **Deletion guards:** Products cannot be deleted if referenced by orders, cart, favorites, or **another product’s assembly** (`ProductsController.Delete`).

---

## Tests

- **`HardwareStore.Tests`** — unit tests for services and mapping helpers; integration tests spin up the MVC host via **`WebApplicationFactory`** (cookie handling may be adjusted for host OS compatibility).
