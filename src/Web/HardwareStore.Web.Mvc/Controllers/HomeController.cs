namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels;
    using HardwareStore.Core.ViewModels.Home;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Mvc;
    using IOFile = System.IO.File;
    using Newtonsoft.Json;

    public class HomeController : Controller
    {
        private readonly IHomeService homeService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IHomeService homeService, ILogger<HomeController> logger)
        {
            this.homeService = homeService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Model = await this.homeService.GetHomeModel();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            var errorModel = new ErrorViewModel
            {
                ErrorMessage = message
            };

            return View(errorModel);
        }
    }
}