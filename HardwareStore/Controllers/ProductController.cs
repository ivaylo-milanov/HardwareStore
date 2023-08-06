namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Product;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly ILogger<ProductController> logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IUserService userService)
        {
            this.productService = productService;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<IActionResult> Details(int productId)
        {
            ProductDetailsModel model;
            try
            {
                model = await this.productService.GetProductDetails(productId);

                model.IsFavorite = await IsFavorite(model.Id);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        private async Task<bool> IsFavorite(int productId)
        {
            var userId = GetUserId();

            if (userId != null)
            {
                var dbFavorites = await userService.GetCustomerFavorites(userId);
                return dbFavorites.Any(f => f.ProductId == productId);
            }

            var favorites = GetFavorites();
            return favorites.Contains(productId);
        }

        private string GetUserId() => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        private ICollection<int> GetFavorites()
            => HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();
    }
}
