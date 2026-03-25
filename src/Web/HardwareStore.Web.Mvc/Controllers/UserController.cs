namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        #region Fields and construction

        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;
        private readonly ILogger<UserController> logger;

        public UserController(
            SignInManager<Customer> signInManager,
            UserManager<Customer> userManager,
            ILogger<UserController> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
                var result = await this.LoginInternalAsync(model);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }
            catch (InvalidOperationException ex)
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
                var result = await this.LoginInternalAsync(model);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }
            catch (InvalidOperationException ex)
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
                result = await this.RegisterInternalAsync(model);
            }
            catch (InvalidOperationException ex)
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
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
        }

        #endregion

        #region Private helpers

        private async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginInternalAsync(LoginFormModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.AccountNotFound);
            }

            return await this.signInManager.PasswordSignInAsync(user, model.Password, false, false);
        }

        private async Task<IdentityResult> RegisterInternalAsync(RegisterFormModel model)
        {
            var user = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                City = model.City,
                Area = model.Area,
                Address = model.Address,
            };

            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        #endregion
    }
}
