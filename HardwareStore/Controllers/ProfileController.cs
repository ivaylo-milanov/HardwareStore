namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public async Task<IActionResult> Index()
        {
            ProfileViewModel model;
            try
            {
                model = await this.profileService.GetProfileModel();
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }
    }
}
