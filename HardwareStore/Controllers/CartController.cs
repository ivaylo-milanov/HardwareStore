namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly ILogger<CartController> logger;
        private readonly IShoppingCartService shoppingCartService;

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
                    shoppingCart = await this.shoppingCartService.GetDatabaseShoppingCartAsync();
                }
                else
                {
                    shoppingCart = await this.shoppingCartService.GetSessionShoppingCartAsync();
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

            return View(shoppingCart);
        }

        public async Task<IActionResult> AddToShoppingCart(int productId, int quantity)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.AddToDatabaseShoppingCartAsync(productId, quantity);
                }
                else
                {
                    await this.shoppingCartService.AddToSessionShoppingCartAsync(productId, quantity);
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
                    await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(productId);
                }
                else
                {
                    await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(productId);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseItemQuantity([FromBody] int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(productId);
                }
                else
                {
                    await this.shoppingCartService.DecreaseSessionItemQuantityAsync(productId);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IncreaseItemQuantity([FromBody] int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(productId);
                }
                else
                {
                    await this.shoppingCartService.IncreaseSessionItemQuantityAsync(productId);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateItemQuantity([FromBody] ShoppingCartUpdateModel model)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(model);
                }
                else
                {
                    await this.shoppingCartService.UpdateSessionItemQuantityAsync(model);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, $"An error occured: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
