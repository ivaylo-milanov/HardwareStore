namespace HardwareStore.Controllers
{
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;

        public UserController(
            SignInManager<Customer> signInManager,
            UserManager<Customer> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
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

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Login!");

            return View(model);
        }

        public IActionResult Register()
        {
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

            Customer user = new Customer
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
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
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
