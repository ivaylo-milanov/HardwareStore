namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;

        public CartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
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
            catch (Exception)
            {
                throw;
            }

            if (TempData["ErrorMessage"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
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
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (InvalidOperationException error)
            {
                TempData["ErrorMessage"] = error.Message;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromShoppingCart([FromBody] int productId)
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
            catch (ArgumentNullException)
            {
                throw;
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
            catch (ArgumentNullException)
            {
                throw;
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
            catch (ArgumentNullException)
            {
                throw;
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
            catch (ArgumentNullException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
