namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;
        private readonly ILogger<ProfileController> logger;

        public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
        {
            this.profileService = profileService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ProfileViewModel model;
            try
            {
                model = await this.profileService.GetProfileModel(GetUserId());
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View(model);
        }

        private string GetUserId() => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
