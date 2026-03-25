namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService favoriteService;
        private readonly ILogger<FavoriteController> logger;

        public FavoriteController(IFavoriteService favoriteService, ILogger<FavoriteController> logger)
        {
            this.favoriteService = favoriteService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var favorites = await this.favoriteService.GetDatabaseFavoriteAsync(this.User.GetUserId());
                return this.View(favorites);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorite(int productId, string? returnUrl = null)
        {
            try
            {
                await this.favoriteService.AddToDatabaseFavoriteAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            if (!string.IsNullOrEmpty(returnUrl) && this.Url.IsLocalUrl(returnUrl))
            {
                return this.LocalRedirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorite(int productId, string? returnUrl = null)
        {
            try
            {
                await this.favoriteService.RemoveFromDatabaseFavoriteAsync(productId, this.User.GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            if (!string.IsNullOrEmpty(returnUrl) && this.Url.IsLocalUrl(returnUrl))
            {
                return this.LocalRedirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
