namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class FavoriteController : Controller
    {
        private readonly IFavoriteService favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            this.favoriteService = favoriteService;
        }

        public IActionResult Index()
        {
            return View(new List<ProductViewModel>());
        }

        public async Task<IActionResult> AddToFavorite(int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.favoriteService.AddToDatabaseFavoriteAsync(productId);
                }
                else
                {
                    await this.favoriteService.AddToSessionFavoriteAsync(productId);
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromFavorite(int productId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    await this.favoriteService.RemoveFromDatabaseFavoriteAsync(productId);
                }
                else
                {
                    await this.favoriteService.RemoveFromSessionFavoriteAsync(productId);
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