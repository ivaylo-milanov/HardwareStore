namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly ILogger<CartController> logger;

        public CartController(IShoppingCartService shoppingCartService, ILogger<CartController> logger)
        {
            this.shoppingCartService = shoppingCartService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ShoppingCartViewModel shoppingCart;
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    shoppingCart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(HttpContext.User.GetUserId());
                }
                else
                {
                    shoppingCart = await this.shoppingCartService.GetSessionShoppingCartAsync(GetShoppingCart());
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }

            if (TempData["ErrorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["ErrorMessage"]!.ToString()!);
            }

            if (shoppingCart.TotalCartPrice == 0)
            {
                ModelState.AddModelError(string.Empty, "To make an order, it is necessary that cart total amount is worth more than $8");
            }

            return View(shoppingCart);
        }

        public async Task<IActionResult> AddToShoppingCart(int productId, int quantity)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.AddToDatabaseShoppingCartAsync(productId, quantity, HttpContext.User.GetUserId());
                }
                else
                {
                    var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(productId, quantity, GetShoppingCart());
                    SetShoppingCart(cart);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(productId, HttpContext.User.GetUserId());
                }
                else
                {
                    var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(productId, GetShoppingCart());
                    SetShoppingCart(cart);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseItemQuantity(int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(productId, HttpContext.User.GetUserId());
                }
                else
                {
                    var cart = await this.shoppingCartService.DecreaseSessionItemQuantityAsync(productId, GetShoppingCart());
                    SetShoppingCart(cart);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IncreaseItemQuantity(int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(productId, HttpContext.User.GetUserId());
                }
                else
                {
                    var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(productId, GetShoppingCart());
                    SetShoppingCart(cart);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateItemQuantity(int quantity, int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(quantity, productId, HttpContext.User.GetUserId());
                }
                else
                {
                    var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(quantity, productId, GetShoppingCart());
                    SetShoppingCart(cart);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        private void SetShoppingCart(ICollection<ShoppingCartExportModel> shoppings)
            => HttpContext.Session.Set("Shopping Cart", shoppings);

        private ICollection<ShoppingCartExportModel> GetShoppingCart()
            => HttpContext.Session.Get<ICollection<ShoppingCartExportModel>>("Shopping Cart") ?? new List<ShoppingCartExportModel>();
    }
}
