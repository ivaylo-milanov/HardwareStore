# Solution source inventory

Verbose catalog of **non-generated** source under [`src/`](../src/). Use this with [features-and-functionality.md](features-and-functionality.md), [api-endpoints.md](api-endpoints.md), and [ui-views.md](ui-views.md).

**EF Core artifacts:** For each migration class `NNN_Name.cs`, there is a paired **`NNN_Name.Designer.cs`** containing auto-generated model metadata. Those Designer files are **not** listed individually. **[`HardwareStoreDbContextModelSnapshot.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Data/Migrations/HardwareStoreDbContextModelSnapshot.cs)** is the consolidated EF model snapshot; treat it as generated.

---

## Repository root

| File | Role |
|------|------|
| [`docker-compose.yml`](../docker-compose.yml) | Local **SQL Server 2022** container: `MSSQL_SA_PASSWORD` required, optional `SQL_PORT` (default 1433), volume `hardwarestore_sql_data`, **healthcheck** via `sqlcmd`, `linux/amd64` for Apple Silicon. Does not run the web apps. |

---

## HardwareStore.Common

| File | Role |
|------|------|
| [`ExceptionMessages.cs`](../src/Common/HardwareStore.Common/ExceptionMessages.cs) | User-facing / service exception strings: product/user/cart not found, stock message template, empty cart, missing connection string. Used by **Core services** and **startup** (DB config). |
| [`GlobalConstants.cs`](../src/Common/HardwareStore.Common/GlobalConstants.cs) | Validation length limits for categories, orders, products, customers, manufacturers, assembly custom role label. Referenced by **entities**, **ViewModels**, and **API request DTOs**. |
| [`LogMessages.cs`](../src/Common/HardwareStore.Common/LogMessages.cs) | Structured log templates for catalog, search, cart, checkout, orders, favorites, profile, home, auth failures. Used by **MVC** and **API** controllers. |

---

## HardwareStore.Infrastructure.Models

| File | Role |
|------|------|
| [`Category.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Category.cs) | Product category: `Name`, `CategoryGroup`, **`AssemblySlot`** (`CategoryAssemblySlot`), navigation to `Products`. |
| [`Product.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Product.cs) | Core sellable item: pricing, stock, refs, `CategoryId`, optional `ManufacturerId`, **`Options`** JSON string, navigations to orders/cart/favorites, **`AssemblyComponents`** (as parent BOM), **`UsedInAssemblies`** (as component). |
| [`Customer.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Customer.cs) | **IdentityUser** subclass: profile fields (`FirstName`, `LastName`, `City`, `Area`, `Address`), collections for `Orders`, `ShoppingCartItems`, `Favorites`. |
| [`Order.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Order.cs) | Order header: `Guid` PK, totals, `OrderStatus`, `PaymentMethod`, shipping/billing fields, `CustomerId`, `ProductsOrders` lines. |
| [`ProductOrder.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/ProductOrder.cs) | Order line: `OrderId`, `ProductId`, `Quantity`, prices. |
| [`ProductAssemblyComponent.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/ProductAssemblyComponent.cs) | BOM row: `AssemblyProductId`, `ComponentProductId`, **`Role`** string (e.g. CPU, GPU, custom label), `Quantity`, `SortOrder`. |
| [`Manufacturer.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Manufacturer.cs) | Brand: `Name`, `Products`. |
| [`ShoppingCartItem.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/ShoppingCartItem.cs) | Cart line: `CustomerId`, `ProductId`, `Quantity`. |
| [`Favorite.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Favorite.cs) | Favorite link: `CustomerId`, `ProductId`. |
| [`Enums/CategoryGroup.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Enums/CategoryGroup.cs) | `Hardware` vs `Peripherals` (nav grouping). |
| [`Enums/CategoryAssemblySlot.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Enums/CategoryAssemblySlot.cs) | Standard BOM slot for a category (`None`, `Cpu`…`Cooler` with gap at 8 to align with admin role kinds). |
| [`Enums/OrderStatus.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Enums/OrderStatus.cs) | Order lifecycle enum. |
| [`Enums/PaymentMethod.cs`](../src/Infrastructure/HardwareStore.Infrastructure.Models/Enums/PaymentMethod.cs) | Payment method enum (e.g. cash, card). |

---

## HardwareStore.Infrastructure

### Data access and SQL Server

| File | Role |
|------|------|
| [`Data/HardwareStoreDbContext.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Data/HardwareStoreDbContext.cs) | **IdentityDbContext&lt;Customer&gt;**; `DbSet`s for `Product`, `Category`, `Order`, `Manufacturer`, `ProductAssemblyComponent`; applies Fluent configurations; calls `base.OnModelCreating` for Identity. |
| [`Data/HardwareStoreDbContextFactory.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Data/HardwareStoreDbContextFactory.cs) | **IDesignTimeDbContextFactory** for EF CLI (migrations); reads connection string env `ConnectionStrings__DefaultConnection` or `DOTNET_CONNECTION_STRING`. |
| [`Data/HardwareStoreSqlServerDbContextOptionsExtensions.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Data/HardwareStoreSqlServerDbContextOptionsExtensions.cs) | **`UseHardwareStoreSqlServer`**: `EnableRetryOnFailure` with extra transient **18401** (script upgrade). |
| [`Common/IRepository.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Common/IRepository.cs) | Abstraction: query sets, CRUD, **`ExecuteInRetryableTransactionAsync`** for retry-safe transactions. |
| [`Common/Repository.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Common/Repository.cs) | EF implementation; in-memory DB runs transaction work without strategy; SQL Server uses execution strategy + `BeginTransactionAsync` inside `ExecuteInRetryableTransactionAsync`. |

### Configurations (`Configurations/`)

| File | Role |
|------|------|
| [`CategoryConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/CategoryConfiguration.cs) | Comments/defaults for `Group`, **`AssemblySlot`** (default `None`). |
| [`ProductConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/ProductConfiguration.cs) | Product table mapping, relationships, delete rules with category/manufacturer. |
| [`ProductOrderConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/ProductOrderConfiguration.cs) | Order line FKs and delete behavior. |
| [`OrderConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/OrderConfiguration.cs) | Order–customer relationship. |
| [`CustomerConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/CustomerConfiguration.cs) | Customer table tweaks. |
| [`ShoppingCartItemConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/ShoppingCartItemConfiguration.cs) | Cart item uniqueness/FKs. |
| [`FavoriteConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/FavoriteConfiguration.cs) | Favorite uniqueness/FKs. |
| [`ProductAssemblyComponentConfiguration.cs`](../src/Infrastructure/HardwareStore.Infrastructure/Configurations/ProductAssemblyComponentConfiguration.cs) | BOM: **Cascade** delete from assembly product; **Restrict** delete on component product if referenced. |

### Migrations (`Data/Migrations/`) — `Up` summary only

| Migration class | What `Up` does |
|-----------------|----------------|
| **`Initial`** | Creates Identity tables, `Categories`, `Manufacturers`, `Products`, `Orders`, `ProductOrders`, `ShoppingCartItems`, `Favorites`, link tables, indexes, FKs (legacy schema including old phone column). |
| **`PhoneColumnRemoved`** | Removes legacy `Phone` column from users in favor of Identity `PhoneNumber`. |
| **`RemoveCharacteristicsTables`** | Drops `Characteristics` / `CharacteristicsNames`; adds **`Products.Options`** JSON column (default `{}`). |
| **`SeedAdminUserAndRole`** | Seeds **Admin** role and admin user (SQL). |
| **`AddCategoryGroup`** | Adds **`Categories.Group`** for Hardware vs Peripherals. |
| **`AddProductAssemblyComponents`** | Creates **`ProductAssemblyComponents`** table and FKs. |
| **`SeedPredefinedHardwareCategories`** | Inserts named hardware categories if missing (Processors, Graphics, …). |
| **`AddCategoryAssemblySlot`** | Adds **`Categories.AssemblySlot`**; SQL `UPDATE`s known seeded names to correct slots. |
| **`AddFullTextSearchCatalogAndIndexes`** | If FTS is installed, creates catalog `product_catalog` and full-text indexes on `Products` / `Manufacturers` (optional; app search uses **LIKE**). |

---

## HardwareStore.Core.ViewModels

### Admin

| File | Role |
|------|------|
| [`ProductFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/ProductFormModel.cs) | Admin create/edit product: core fields + **`Options`** JSON string + **`AssemblyComponents`** list. |
| [`ProductListItemViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/ProductListItemViewModel.cs) | Admin product list row. |
| [`AssemblyComponentInputModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/AssemblyComponentInputModel.cs) | One BOM row: `RoleKind`, `CustomRole`, `ComponentProductId`, `Quantity`. |
| [`AssemblyRoleKind.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/AssemblyRoleKind.cs) | Enum: standard slots + `Custom` + `None`. |
| [`AssemblyRoleMapping.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/AssemblyRoleMapping.cs) | Maps stored role strings ↔ enum; **`AssemblySlotForRole`**; **`ProductCategoryMatchesRole`** using **`CategoryAssemblySlot`**. |
| [`CategoryFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/CategoryFormModel.cs) | Admin category create/edit (`Name`, `Group`). |
| [`ManufacturerFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/ManufacturerFormModel.cs) | Admin manufacturer create/edit. |
| [`AdminCustomersIndexViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/AdminCustomersIndexViewModel.cs) | Paged admin customer list wrapper. |
| [`CustomerListItemViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/CustomerListItemViewModel.cs) | Single customer row for admin index. |
| [`CustomerEditFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Admin/CustomerEditFormModel.cs) | Admin edit customer profile fields. |

### Product / catalog / details

| File | Role |
|------|------|
| [`CatalogPageViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/CatalogPageViewModel.cs) | Catalog or search page: title, category key, keyword, `Catalog`, selected sort order. |
| [`ProductsViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/ProductsViewModel.cs) | Generic container: `Filters` + `Products` list. |
| [`CatalogProductViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/CatalogProductViewModel.cs) | Card row: name, price, manufacturer, parsed `Options` for filters. |
| [`FilterPartialViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/FilterPartialViewModel.cs) | One filter dimension (e.g. manufacturer, option key) for sidebar. |
| [`FilterCategoryModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/FilterCategoryModel.cs) | Filter POST model for MVC. |
| [`ProductViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/ProductViewModel.cs) | Minimal product (e.g. home tiles). |
| [`ProductAttributeExportModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Product/ProductAttributeExportModel.cs) | Key/value attribute for display/API. |
| [`Details/ProductDetailsModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Details/ProductDetailsModel.cs) | Details page/API payload: metadata + `Attributes` + **`AssemblyComponents`** + `IsFavorite`. |
| [`Details/AssemblyComponentModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Details/AssemblyComponentModel.cs) | One line on public details: role, component id/name/ref, price, qty. |
| [`Details/DetailsFavoriteModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Details/DetailsFavoriteModel.cs) | Favorite toggle payload for details UI. |

### Home, order, cart, favorite, profile, user, nav, errors

| File | Role |
|------|------|
| [`Home/HomeViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Home/HomeViewModel.cs) | Newest + most-bought product lists. |
| [`Order/OrderFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Order/OrderFormModel.cs) | Checkout form + `TotalAmount`. |
| [`Order/OrderViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Order/OrderViewModel.cs) | Order history row. |
| [`ShoppingCart/ShoppingCartViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/ShoppingCart/ShoppingCartViewModel.cs) | Cart page: lines + total. |
| [`ShoppingCart/ShoppingCartItemViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/ShoppingCart/ShoppingCartItemViewModel.cs) | One cart line. |
| [`ShoppingCart/ShoppingCartUpdateModel.cs`](../src/Core/HardwareStore.Core.ViewModels/ShoppingCart/ShoppingCartUpdateModel.cs) | Form/API quantity update. |
| [`ShoppingCart/ShoppingCartExportModel.cs`](../src/Core/HardwareStore.Core.ViewModels/ShoppingCart/ShoppingCartExportModel.cs) | Cart serialization shape if used. |
| [`Favorite/FavoriteExportModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Favorite/FavoriteExportModel.cs) | Favorite list item for API/UI. |
| [`Profile/ProfileViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/Profile/ProfileViewModel.cs) | Read-only profile summary. |
| [`User/LoginFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/User/LoginFormModel.cs) | Email + password (MVC + API). |
| [`User/RegisterFormModel.cs`](../src/Core/HardwareStore.Core.ViewModels/User/RegisterFormModel.cs) | Registration fields (MVC + API). |
| [`CategoryNavViewModels.cs`](../src/Core/HardwareStore.Core.ViewModels/CategoryNavViewModels.cs) | **`CategoryNavLinkItem`**, **`CategoryNavGroupViewModel`** for navbar view component. |
| [`ErrorViewModel.cs`](../src/Core/HardwareStore.Core.ViewModels/ErrorViewModel.cs) | Error page request id / message. |

---

## HardwareStore.Core

| File | Role |
|------|------|
| [`Enums/ProductOrdering.cs`](../src/Core/HardwareStore.Core/Enums/ProductOrdering.cs) | Sort options for catalog (used when parsing filter JSON `Order`). |
| [`Services/Contracts/IProductService.cs`](../src/Core/HardwareStore.Core/Services/Contracts/IProductService.cs) | Home, category/search catalog, filters, details, `CategoryExistsAsync`, favorites check. |
| [`Services/ProductService.cs`](../src/Core/HardwareStore.Core/Services/ProductService.cs) | Implements catalog building, **`EF.Functions.Like`** search with escape, JSON filter parsing, **`GetProductDetails`** with assembly includes. |
| [`Services/Contracts/IShoppingCartService.cs`](../src/Core/HardwareStore.Core/Services/Contracts/IShoppingCartService.cs) | Cart CRUD and quantity ops. |
| [`Services/ShoppingCartService.cs`](../src/Core/HardwareStore.Core/Services/ShoppingCartService.cs) | DB-backed cart; stock checks; throws `ArgumentNullException` / `InvalidOperationException` with **`ExceptionMessages`**. |
| [`Services/Contracts/IOrderService.cs`](../src/Core/HardwareStore.Core/Services/Contracts/IOrderService.cs) | Checkout model, place order, list orders. |
| [`Services/OrderService.cs`](../src/Core/HardwareStore.Core/Services/OrderService.cs) | **`OrderAsync`** delegates to **`ExecuteInRetryableTransactionAsync`**; decrements stock, creates `Order` + `ProductOrder`s, clears cart. |
| [`Services/Contracts/IFavoriteService.cs`](../src/Core/HardwareStore.Core/Services/Contracts/IFavoriteService.cs) | List/add/remove favorites. |
| [`Services/FavoriteService.cs`](../src/Core/HardwareStore.Core/Services/FavoriteService.cs) | Favorite persistence. |

---

## HardwareStore.Web.Mvc

### Entry and extensions

| File | Role |
|------|------|
| [`Program.cs`](../src/Web/HardwareStore.Web.Mvc/Program.cs) | **Admin** policy, **global antiforgery**, DB + Identity + services + Razor catalog paths; pipeline: dev migrations endpoint, HTTPS, static files, auth, default + **areas** routes, Razor Pages. |
| [`Extensions/AddDbContextExtension.cs`](../src/Web/HardwareStore.Web.Mvc/Extensions/AddDbContextExtension.cs) | Registers **`HardwareStoreDbContext`** with **`UseHardwareStoreSqlServer`**; requires `DefaultConnection`. |
| [`Extensions/ConfigurateIdentityExtension.cs`](../src/Web/HardwareStore.Web.Mvc/Extensions/ConfigurateIdentityExtension.cs) | Cookie Identity for **`Customer`**, password/lockout settings, EF stores. |
| [`Extensions/AddServicesExtension.cs`](../src/Web/HardwareStore.Web.Mvc/Extensions/AddServicesExtension.cs) | Scoped **Repository** + **Product / Favorite / Cart / Order** services + **`IHttpContextAccessor`**. |
| [`Extensions/AddSearchPathsExtension.cs`](../src/Web/HardwareStore.Web.Mvc/Extensions/AddSearchPathsExtension.cs) | Adds **`/Views/Shared/Catalog/{0}.cshtml`** view location. |
| [`Extensions/AdminConstants.cs`](../src/Web/HardwareStore.Web.Mvc/Extensions/AdminConstants.cs) | Admin role name string for `[Authorize(Roles=…)]`. |
| [`Extensions/UserExtension.cs`](../src/Web/HardwareStore.Web.Mvc/Extensions/UserExtension.cs) | **`ClaimsPrincipal.GetUserId()`** → `NameIdentifier`. |

### Controllers (root `Controllers/`)

| File | Role |
|------|------|
| [`HomeController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/HomeController.cs) | **`Index`**: home view model; **`Error`**: optional message. |
| [`ProductController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/ProductController.cs) | **`Index(category,title)`** → `Catalog` view; **`Filter(category)`** POST applies **`CatalogFilterFormHelper`** JSON; **`Details(productId)`** + favorite flag for signed-in user. |
| [`SearchController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/SearchController.cs) | Search catalog by keyword; filter POST for search results. |
| [`CartController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/CartController.cs) | **`[Authorize]`** cart index, add/remove, inc/dec qty, update quantity form. |
| [`CheckoutController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/CheckoutController.cs) | **`[Authorize]`** checkout form, **place order**, success view. |
| [`OrdersController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/OrdersController.cs) | **`[Authorize]`** order history. |
| [`FavoriteController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/FavoriteController.cs) | **`[Authorize]`** favorites list; add/remove with return URL. |
| [`ProfileController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/ProfileController.cs) | **`[Authorize]`** profile display. |
| [`UserController.cs`](../src/Web/HardwareStore.Web.Mvc/Controllers/UserController.cs) | Login/register GET/POST, **EasyLogin** (dev convenience?), logout; uses **SignInManager** / **UserManager**. |

### Areas/Admin/Controllers

| File | Role |
|------|------|
| [`AdminControllerBase.cs`](../src/Web/HardwareStore.Web.Mvc/Areas/Admin/Controllers/AdminControllerBase.cs) | **`[Area("Admin")]`** + **`[Authorize(Roles = Admin)]`**. |
| [`HomeController.cs`](../src/Web/HardwareStore.Web.Mvc/Areas/Admin/Controllers/HomeController.cs) | Admin landing. |
| [`ProductsController.cs`](../src/Web/HardwareStore.Web.Mvc/Areas/Admin/Controllers/ProductsController.cs) | Product CRUD; **Options** JSON validation; **assembly** load/validate/persist; **delete** guards (orders, cart, favorites, used-in-assembly); **`FillSelectListsAsync`** builds category/manufacturer lists + assembly catalog JSON. |
| [`CategoriesController.cs`](../src/Web/HardwareStore.Web.Mvc/Areas/Admin/Controllers/CategoriesController.cs) | Category CRUD. |
| [`ManufacturersController.cs`](../src/Web/HardwareStore.Web.Mvc/Areas/Admin/Controllers/ManufacturersController.cs) | Manufacturer CRUD + delete guard if products reference. |
| [`CustomersController.cs`](../src/Web/HardwareStore.Web.Mvc/Areas/Admin/Controllers/CustomersController.cs) | Paged customer list; edit customer profile. |

### UI helpers and components

| File | Role |
|------|------|
| [`Helpers/CatalogFilterFormHelper.cs`](../src/Web/HardwareStore.Web.Mvc/Helpers/CatalogFilterFormHelper.cs) | Builds **filter JSON** from `IFormCollection` for category/search POST; parses selected filters for reposting checkboxes. |
| [`ViewComponents/CategoriesNavViewComponent.cs`](../src/Web/HardwareStore.Web.Mvc/ViewComponents/CategoriesNavViewComponent.cs) | Loads **Peripherals** and **Hardware** categories from DB; returns **`CategoryNavGroupViewModel`** list (skips empty groups). |

### Identity area

| Path | Role |
|------|------|
| [`Areas/Identity/Pages/_ViewStart.cshtml`](../src/Web/HardwareStore.Web.Mvc/Areas/Identity/Pages/_ViewStart.cshtml) | Only file under **Identity** in repo; layout hook for scaffolded Identity pages if added later. |

---

## HardwareStore.Web.Api

| File | Role |
|------|------|
| [`Program.cs`](../src/Web/HardwareStore.Web.Api/Program.cs) | Validates **Jwt** config; **Swagger** + Bearer security; **JWT Bearer** auth default; **Identity** + EF stores; **domain services**; HTTPS; **MapControllers** (no CORS in repo). |
| [`Options/JwtOptions.cs`](../src/Web/HardwareStore.Web.Api/Options/JwtOptions.cs) | **IOptions** binding: `Key`, `Issuer`, `Audience`, `ExpireHours`. |
| [`Services/IJwtTokenService.cs`](../src/Web/HardwareStore.Web.Api/Services/IJwtTokenService.cs) | Token creation contract. |
| [`Services/JwtTokenService.cs`](../src/Web/HardwareStore.Web.Api/Services/JwtTokenService.cs) | Builds **JwtSecurityToken** with sub/email/name claims; HMAC-SHA256 signing. |
| [`Extensions/ServiceCollectionExtensions.cs`](../src/Web/HardwareStore.Web.Api/Extensions/ServiceCollectionExtensions.cs) | **`AddHardwareStoreDbContext`**, **`AddHardwareStoreDomainServices`** (same services as MVC). |
| [`Extensions/CustomerIdentityOptions.cs`](../src/Web/HardwareStore.Web.Api/Extensions/CustomerIdentityOptions.cs) | Shared Identity password/user options for **Customer**. |
| [`Extensions/ClaimsPrincipalExtensions.cs`](../src/Web/HardwareStore.Web.Api/Extensions/ClaimsPrincipalExtensions.cs) | **`GetUserId()`** for API controllers. |
| [`Models/ApiModels.cs`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs) | **AuthTokenResponse**, **CatalogFilterRequest**, **SearchFilterRequest**, **AddCartItemRequest**, **AddFavoriteRequest**, **ProfileUpdateRequest**. |

### API `Controllers/`

| File | Role |
|------|------|
| [`AuthController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/AuthController.cs) | **POST login/register** `[AllowAnonymous]` → JWT; **POST logout** `[Authorize]` (no server-side token revoke). |
| [`HomeController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/HomeController.cs) | **GET api/Home** → **`HomeViewModel`**. |
| [`CategoryCatalogController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/CategoryCatalogController.cs) | **GET/POST filter** under **`api/categories/{category}/catalog`**; anonymous. |
| [`SearchController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/SearchController.cs) | **GET api/search** + **POST api/search/filter**; anonymous. |
| [`ProductDetailsController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/ProductDetailsController.cs) | **GET api/products/{id}`** `[AllowAnonymous]`; sets **`IsFavorite`** if authenticated. |
| [`CartController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/CartController.cs) | **Class-level `[Authorize]`**; full cart REST surface. |
| [`CheckoutController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/CheckoutController.cs) | **`[Authorize]`** GET checkout model, POST place order. |
| [`OrdersController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/OrdersController.cs) | **`[Authorize]`** GET order list. |
| [`FavoritesController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/FavoritesController.cs) | **`[Authorize]`** GET/POST/DELETE favorites. |
| [`ProfileController.cs`](../src/Web/HardwareStore.Web.Api/Controllers/ProfileController.cs) | **`[Authorize]`** GET profile, PUT update via **UserManager**. |

---

## HardwareStore.Tests

| File | Role |
|------|------|
| [`Usings.cs`](../src/Tests/HardwareStore.Tests/Usings.cs) | Global usings for NUnit etc. |
| [`Mocking/TestRepository.cs`](../src/Tests/HardwareStore.Tests/Mocking/TestRepository.cs) | **`GetRepository()`**: in-memory **HardwareStoreDbContext**, **EnsureCreated**, seed manufacturers/categories (with **AssemblySlot** for CPU/GPU test products), products (incl. prebuilt with assembly lines), users/carts/favorites/orders. |
| [`Mocking/ShoppingCartItemViewModelComparer.cs`](../src/Tests/HardwareStore.Tests/Mocking/ShoppingCartItemViewModelComparer.cs) | Equality comparer for cart VM tests. |
| [`Mocking/FavoriteExportModelComparer.cs`](../src/Tests/HardwareStore.Tests/Mocking/FavoriteExportModelComparer.cs) | Equality comparer for favorite export models. |
| [`Mocking/ProductOrderComparer.cs`](../src/Tests/HardwareStore.Tests/Mocking/ProductOrderComparer.cs) | Equality comparer for order lines. |
| [`Unit/ProductServiceTest.cs`](../src/Tests/HardwareStore.Tests/Unit/ProductServiceTest.cs) | Product service catalog/home/filter behavior. |
| [`Unit/ProductServiceDetailsTest.cs`](../src/Tests/HardwareStore.Tests/Unit/ProductServiceDetailsTest.cs) | **`GetProductDetails`** including assembly components. |
| [`Unit/ShoppingCartServiceTest.cs`](../src/Tests/HardwareStore.Tests/Unit/ShoppingCartServiceTest.cs) | Cart add/update/remove/stock edge cases. |
| [`Unit/OrderServiceTest.cs`](../src/Tests/HardwareStore.Tests/Unit/OrderServiceTest.cs) | Order placement and history. |
| [`Unit/FavoriteServiceTest.cs`](../src/Tests/HardwareStore.Tests/Unit/FavoriteServiceTest.cs) | Favorite add/remove/list. |
| [`Unit/AssemblyRoleMappingTests.cs`](../src/Tests/HardwareStore.Tests/Unit/AssemblyRoleMappingTests.cs) | **`AssemblyRoleMapping`** slot/role rules and persist roundtrip. |
| [`Integration/HardwareStoreWebApplicationFactory.cs`](../src/Tests/HardwareStore.Tests/Integration/HardwareStoreWebApplicationFactory.cs) | **WebApplicationFactory** for **MVC `Program`**: overrides connection string, can swap to **in-memory EF** for tests. |
| [`Integration/MvcIntegrationTests.cs`](../src/Tests/HardwareStore.Tests/Integration/MvcIntegrationTests.cs) | HTTP-level tests against MVC pipeline. |
| [`Integration/ProductionErrorPageTests.cs`](../src/Tests/HardwareStore.Tests/Integration/ProductionErrorPageTests.cs) | Error handling under **Production** environment. |

---

## Related generated / tooling files (not duplicated here)

- All **`*.Designer.cs`** under `Migrations/` — EF migration metadata.
- **`HardwareStoreDbContextModelSnapshot.cs`** — current model snapshot for migrations.
