namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        #region Fields and construction

        private readonly IAuthenticationService authenticationService;
        private readonly ILogger<UserController> logger;

        public UserController(IAuthenticationService authenticationService, ILogger<UserController> logger)
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        #endregion

        #region Login

        [HttpPost]
        public async Task<IActionResult> EasyLogin(LoginFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Login));
            }

            try
            {
                var result = await this.authenticationService.LoginAsync(model);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.AuthenticationOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return this.RedirectToAction(nameof(this.Login));
        }

        public IActionResult Login()
        {
            if (this.User?.Identity?.IsAuthenticated ?? false)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(new LoginFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var result = await this.authenticationService.LoginAsync(model);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }
            catch (ArgumentNullException ex)
            {
                this.ModelState.AddModelError("", "Invalid Login!");
                this.logger.LogError(ex, LogMessages.AuthenticationOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return this.View(model);
        }

        #endregion

        #region Registration

        public IActionResult Register()
        {
            if (this.User?.Identity?.IsAuthenticated ?? false)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(new RegisterFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            IdentityResult result;
            try
            {
                result = await this.authenticationService.RegisterAsync(model);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.AuthenticationOperationFailed);
                return this.RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            if (result.Succeeded)
            {
                return this.RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                this.ModelState.AddModelError("", item.Description);
            }

            return this.View(model);
        }

        #endregion

        #region Session

        public async Task<IActionResult> Logout()
        {
            await this.authenticationService.LogoutAsync();

            return this.RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
