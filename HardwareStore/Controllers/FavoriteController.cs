namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Http;
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
            var favorites = GetFavorites();

            return View(favorites);
        }

        public async Task<IActionResult> AddToFavorite(int id)
        {
            List<int> favorites;
            try
            {
                favorites = await this.favoriteService.AddToFavoriteAsync(GetFavorites(), id);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            HttpContext.Session.Set("Favorites", favorites);

            return View();
        }

        public async Task<IActionResult> RemoveFromFavorite(int id)
        {
            List<int> favorites;
            try
            {
                favorites = await this.favoriteService.RemoveFromFavoriteAsync(GetFavorites(), id);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            HttpContext.Session.Set("Favorites", favorites);

            return View();
        }

        private List<int> GetFavorites()
            => HttpContext.Session.Get<List<int>>("Favorites") ?? new List<int>();
    }
}