namespace HardwareStore.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var newProducts = await this.homeService.GetNewProducts();
            var mostBoughtProducts = await this.homeService.GetMostBoughtProducts();

            HomeViewModel model = new HomeViewModel
            {
                NewestProducts = newProducts,
                MostBoughtProducts = mostBoughtProducts
            };

            return View(model);
        }
    }
}