namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly IAuthenticationService authenticationService;
        private readonly ILogger<UserController> logger;

        public UserController(IAuthenticationService authenticationService, ILogger<UserController> logger)
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        private async Task<IActionResult> RedirectHomeAfterMergingSessionAsync()
        {
            var userId = this.User.GetUserId();
            var favorites = this.HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();
            var cart = this.HttpContext.Session.Get<ICollection<ShoppingCartExportModel>>("Shopping Cart") ?? new List<ShoppingCartExportModel>();

            try
            {
                await this.authenticationService.MergeSessionCartAndFavoritesAsync(userId, favorites, cart);
                this.HttpContext.Session.Remove("Shopping Cart");
                this.HttpContext.Session.Remove("Favorite");
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EasyLogin(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                var result = await authenticationService.LoginAsync(model);

                if (result.Succeeded)
                {
                    return await this.RedirectHomeAfterMergingSessionAsync();
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

            try
            {
                var result = await authenticationService.LoginAsync(model);

                if (result.Succeeded)
                {
                    return await this.RedirectHomeAfterMergingSessionAsync();
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

            IdentityResult result;
            try
            {
                result = await authenticationService.RegisterAsync(model);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            if (result.Succeeded)
            {
                return await this.RedirectHomeAfterMergingSessionAsync();
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await authenticationService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}