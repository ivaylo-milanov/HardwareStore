namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.ViewModels.Profile;
    using HardwareStore.Extensions;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ProfileController : Controller
    {
        #region Fields and construction

        private readonly UserManager<Customer> userManager;
        private readonly ILogger<ProfileController> logger;

        public ProfileController(UserManager<Customer> userManager, ILogger<ProfileController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        #endregion

        #region Profile

        public async Task<IActionResult> Index()
        {
            var customer = await this.userManager.FindByIdAsync(this.User.GetUserId());
            if (customer == null)
            {
                this.logger.LogError(LogMessages.ProfileUnknownUser, this.User.GetUserId());
                return this.RedirectToAction("Error", "Home");
            }

            var model = new ProfileViewModel
            {
                FullName = $"{customer.FirstName} {customer.LastName}",
                City = customer.City,
                Address = customer.Address,
                Email = customer.Email ?? string.Empty,
            };

            return this.View(model);
        }

        #endregion
    }
}
