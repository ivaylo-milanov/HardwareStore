namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CartController : Controller
    {
        #region Fields and construction

        private readonly IShoppingCartService shoppingCartService;
        private readonly ILogger<CartController> logger;

        public CartController(IShoppingCartService shoppingCartService, ILogger<CartController> logger)
        {
            this.shoppingCartService = shoppingCartService;
            this.logger = logger;
        }

        #endregion

        #region Cart view

        public async Task<IActionResult> Index()
        {
            ShoppingCartViewModel shoppingCart;
            try
            {
                shoppingCart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.RedirectToAction("Error", "Home");
            }

            if (this.TempData["ErrorMessage"] != null)
            {
                this.ModelState.AddModelError(string.Empty, this.TempData["ErrorMessage"]!.ToString()!);
            }

            if (shoppingCart.TotalCartPrice == 0)
            {
                this.ModelState.AddModelError(string.Empty, "To make an order, it is necessary that cart total amount is worth more than $8");
            }

            return this.View(shoppingCart);
        }

        public async Task<IActionResult> AddToShoppingCart(int productId, int quantity)
        {
            try
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(productId, quantity, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.RedirectToAction("Error", "Home");
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                this.TempData["ErrorMessage"] = ex.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromShoppingCart(int productId)
        {
            try
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseItemQuantity(int productId)
        {
            try
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseItemQuantity(int productId)
        {
            try
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                this.TempData["ErrorMessage"] = ex.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItemQuantity([FromForm] ShoppingCartUpdateModel model)
        {
            try
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(model.Quantity, model.ProductId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                this.TempData["ErrorMessage"] = ex.Message;
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        #endregion
    }
}
