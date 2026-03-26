# REST API reference (`HardwareStore.Web.Api`)

Base URL in development is typically `https://localhost:7133` (see [`launchSettings.json`](../src/Web/HardwareStore.Web.Api/Properties/launchSettings.json)). All routes below are relative to that origin.

**Authentication:** JWT Bearer (`Authorization: Bearer <token>`). Controllers or actions marked **JWT** require a valid token unless noted **Anonymous**.

**JSON:** Controllers use `PropertyNamingPolicy = null` (PascalCase property names in JSON by default).

Request/response DTOs shared with MVC ViewModels live in **HardwareStore.Core.ViewModels**; API-specific bodies are in [`ApiModels.cs`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs).

---

## Endpoints

| Method | Route | Auth | Body / query | Response / notes |
|--------|-------|------|----------------|------------------|
| POST | `api/Auth/login` | Anonymous | [`LoginFormModel`](../src/Core/HardwareStore.Core.ViewModels/User/LoginFormModel.cs) | 200 **`AuthTokenResponse`**; 401 bad credentials |
| POST | `api/Auth/register` | Anonymous | [`RegisterFormModel`](../src/Core/HardwareStore.Core.ViewModels/User/RegisterFormModel.cs) | 201 **`AuthTokenResponse`**; 400 validation |
| POST | `api/Auth/logout` | JWT | (none) | 200 OK (stateless; client discards token) |
| GET | `api/Home` | Anonymous | — | [`HomeViewModel`](../src/Core/HardwareStore.Core.ViewModels/Home/HomeViewModel.cs) |
| GET | `api/categories/{category}/catalog` | Anonymous | — | [`CatalogPageViewModel`](../src/Core/HardwareStore.Core.ViewModels/Product/CatalogPageViewModel.cs); 404 unknown category |
| POST | `api/categories/{category}/catalog/filter` | Anonymous | [`CatalogFilterRequest`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs) (`FilterJson`, `Order`) | `CatalogPageViewModel` with filtered products |
| GET | `api/search?keyword=` | Anonymous | Query `keyword` optional | `CatalogPageViewModel` (search) |
| POST | `api/search/filter` | Anonymous | [`SearchFilterRequest`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs) (`Keyword`, `FilterJson`, `Order`) | `CatalogPageViewModel` with filtered search |
| GET | `api/products/{productId}` | Anonymous | — | [`ProductDetailsModel`](../src/Core/HardwareStore.Core.ViewModels/Details/ProductDetailsModel.cs); `IsFavorite` false if anonymous; 404 missing product |
| GET | `api/Cart` | JWT | — | [`ShoppingCartViewModel`](../src/Core/HardwareStore.Core.ViewModels/ShoppingCart/ShoppingCartViewModel.cs) |
| POST | `api/Cart/items` | JWT | [`AddCartItemRequest`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs) | 204; 400 stock / not found |
| DELETE | `api/Cart/items/{productId}` | JWT | — | 204 |
| POST | `api/Cart/items/{productId}/decrease` | JWT | — | 204 |
| POST | `api/Cart/items/{productId}/increase` | JWT | — | 204; 400 stock |
| PUT | `api/Cart/items` | JWT | [`ShoppingCartUpdateModel`](../src/Core/HardwareStore.Core.ViewModels/ShoppingCart/ShoppingCartUpdateModel.cs) | 204 |
| GET | `api/Checkout` | JWT | — | [`OrderFormModel`](../src/Core/HardwareStore.Core.ViewModels/Order/OrderFormModel.cs) with `TotalAmount`; 400 empty cart |
| POST | `api/Checkout/place` | JWT | [`OrderFormModel`](../src/Core/HardwareStore.Core.ViewModels/Order/OrderFormModel.cs) | 204 success; 400 validation / business rules; server refreshes `TotalAmount` from cart |
| GET | `api/Orders` | JWT | — | `IEnumerable<`[`OrderViewModel`](../src/Core/HardwareStore.Core.ViewModels/Order/OrderViewModel.cs)`>` |
| GET | `api/Favorites` | JWT | — | `ICollection<`[`FavoriteExportModel`](../src/Core/HardwareStore.Core.ViewModels/Favorite/FavoriteExportModel.cs)`>` |
| POST | `api/Favorites` | JWT | [`AddFavoriteRequest`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs) | 204 |
| DELETE | `api/Favorites/{productId}` | JWT | — | 204 |
| GET | `api/Profile` | JWT | — | [`ProfileViewModel`](../src/Core/HardwareStore.Core.ViewModels/Profile/ProfileViewModel.cs); 404 unknown user |
| PUT | `api/Profile` | JWT | [`ProfileUpdateRequest`](../src/Web/HardwareStore.Web.Api/Models/ApiModels.cs) | 204; 400 Identity validation |

**Note:** `CartController` uses route prefix **`api/Cart`** (PascalCase controller name in URL).

---

## Configuration

- **`Jwt`** section: `Key` (≥32 chars), `Issuer`, `Audience`, `ExpireHours` — see [`JwtOptions.cs`](../src/Web/HardwareStore.Web.Api/Options/JwtOptions.cs). Missing or short key fails startup.

## Swagger

Development enables **Swagger UI** at `/swagger` with Bearer security scheme — see [`Program.cs`](../src/Web/HardwareStore.Web.Api/Program.cs).

## CORS

Not configured in the current codebase; browser SPAs on another origin need a **CORS policy** or a **reverse proxy** unless they call the API same-origin.

## Further reading

- [solution-inventory.md](solution-inventory.md) — per-controller file references  
- [features-and-functionality.md](features-and-functionality.md) — feature-level behavior  
