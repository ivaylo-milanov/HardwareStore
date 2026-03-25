namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Web.Api.Extensions;
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Web.Api.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly ILogger<CartController> logger;

        public CartController(IShoppingCartService shoppingCartService, ILogger<CartController> logger)
        {
            this.shoppingCartService = shoppingCartService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartViewModel>> Get()
        {
            try
            {
                return this.Ok(await this.shoppingCartService.GetDatabaseShoppingCartAsync(this.User.GetUserId()));
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(
                    request.ProductId,
                    request.Quantity,
                    this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }

        [HttpPost("items/{productId:int}/decrease")]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            try
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }

        [HttpPost("items/{productId:int}/increase")]
        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            try
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateQuantity([FromBody] ShoppingCartUpdateModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(
                    model.Quantity,
                    model.ProductId,
                    this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, LogMessages.CartOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }
    }
}
