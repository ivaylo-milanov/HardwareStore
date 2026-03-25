namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Web.Api.Extensions;
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Web.Api.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService favoriteService;
        private readonly ILogger<FavoritesController> logger;

        public FavoritesController(IFavoriteService favoriteService, ILogger<FavoritesController> logger)
        {
            this.favoriteService = favoriteService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<FavoriteExportModel>>> Get()
        {
            try
            {
                var favorites = await this.favoriteService.GetDatabaseFavoriteAsync(this.User.GetUserId());
                return this.Ok(favorites);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.FavoriteOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddFavoriteRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                await this.favoriteService.AddToDatabaseFavoriteAsync(request.ProductId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.FavoriteOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Remove(int productId)
        {
            try
            {
                await this.favoriteService.RemoveFromDatabaseFavoriteAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.FavoriteOperationFailed);
                return this.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Message);
            }

            return this.NoContent();
        }
    }
}
