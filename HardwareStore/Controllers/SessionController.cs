namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;
        private readonly ILogger<SessionController> logger;

        public SessionController(ISessionService sessionService, ILogger<SessionController> logger)
        {
            this.sessionService = sessionService;
            this.logger = logger;
        }

        public async Task<IActionResult> AddFromSessionToDb()
        {
            try
            {
                await this.sessionService.AddToDatabase(User.GetUserId(), GetFavorites(), GetShoppingCart());

                RemoveFromSession();
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction("Index", "Home");
        }

        private ICollection<ShoppingCartExportModel> GetShoppingCart()
            => HttpContext.Session.Get<ICollection<ShoppingCartExportModel>>("Shopping Cart") ?? new List<ShoppingCartExportModel>();

        private ICollection<int> GetFavorites()
            => HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();

        private void RemoveFromSession()
        {
            HttpContext.Session.Remove("Shopping Cart");
            HttpContext.Session.Remove("Favorite");
        }
    }
}
