namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IProductService productService, ILogger<HomeController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await this.productService.GetHomeViewModelAsync());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
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