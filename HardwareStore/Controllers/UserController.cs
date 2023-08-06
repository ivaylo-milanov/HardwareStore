namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.ViewModels.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> EasyLogin(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            var cartItems = GetShoppingCart();
            var favorites = GetFavorites();

            try
            {
                var result = await userService.LoginAsync(model, cartItems, favorites);

                if (result.Succeeded)
                {
                    RemoveShoppingCart();
                    RemoveFavorites();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            LoginFormModel model = new LoginFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cartItems = GetShoppingCart();
            var favorites = GetFavorites();

            try
            {
                var result = await userService.LoginAsync(model, cartItems, favorites);

                if (result.Succeeded)
                {
                    RemoveShoppingCart();
                    RemoveFavorites();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", "Invalid Login!");
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            RegisterFormModel model = new RegisterFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cartItems = GetShoppingCart();
            var favorites = GetFavorites();

            IdentityResult result;
            try
            {
                result = await this.userService.RegisterAsync(model, cartItems, favorites);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            if (result.Succeeded)
            {
                RemoveShoppingCart();
                RemoveFavorites();
                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await userService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        private ICollection<ShoppingCartExportModel> GetShoppingCart()
            => HttpContext.Session.Get<ICollection<ShoppingCartExportModel>>("Shopping Cart") ?? new List<ShoppingCartExportModel>();

        private ICollection<int> GetFavorites()
            => HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();

        private void RemoveShoppingCart()
            => HttpContext.Session.Remove("Shopping Cart");

        private void RemoveFavorites()
            => HttpContext.Session.Remove("Favorite");
    }
}