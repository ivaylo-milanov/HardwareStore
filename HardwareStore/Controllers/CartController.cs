namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCard;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly IShoppingCardService shoppingCardService;

        public CartController(IShoppingCardService shoppingCardService)
        {
            this.shoppingCardService = shoppingCardService;
        }

        public IActionResult Index()
        {
            //var shoppingCard = GetShoppingCard();

            return View();
        }

        public async Task<IActionResult> AddToShoppingCard(int id, int quantity)
        {
            List<ShoppingCardModel> shoppingCard;
            try
            {
                shoppingCard = await this.shoppingCardService.AddToShoppingCardAsync(GetShoppingCard(), id, quantity);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            HttpContext.Session.Set("Shopping Card", shoppingCard);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromFavorite(int id)
        {
            List<ShoppingCardModel> shoppingCard;
            try
            {
                shoppingCard = await this.shoppingCardService.RemoveFromShoppingCardAsync(GetShoppingCard(), id);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            HttpContext.Session.Set("Shopping Card", shoppingCard);

            return RedirectToAction("Index");
        }

        private List<ShoppingCardModel> GetShoppingCard()
            => HttpContext.Session.Get<List<ShoppingCardModel>>("Shopping Cart") ?? new List<ShoppingCardModel>();
    }
}
