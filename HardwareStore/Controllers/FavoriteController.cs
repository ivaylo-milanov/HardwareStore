namespace HardwareStore.Controllers
{
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class FavoriteController : Controller
    {
        public IActionResult AddToFavorite(int id)
        {
            var favorite = GetFavorites();

            favorite.Add(id);

            HttpContext.Session.Set("Favorite", favorite);

            return View();
        }

        public IActionResult RemoveFromFavorite(int id)
        {
            var favorite = GetFavorites();

            favorite.Remove(id);

            HttpContext.Session.Set("Favorite", favorite);

            return View();
        }

        private List<int> GetFavorites()
            => HttpContext.Session.Get<List<int>>("Favorite") ?? new List<int>();
    }
}
