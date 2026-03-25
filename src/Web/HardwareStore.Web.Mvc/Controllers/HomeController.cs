namespace HardwareStore.Web.Mvc.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        #region Fields and construction

        private readonly IProductService productService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IProductService productService,
            IWebHostEnvironment hostEnvironment,
            ILogger<HomeController> logger)
        {
            this.productService = productService;
            this.hostEnvironment = hostEnvironment;
            this.logger = logger;
        }

        #endregion

        #region Pages

        public async Task<IActionResult> Index()
        {
            try
            {
                return this.View(await this.productService.GetHomeViewModelAsync());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, LogMessages.HomePageLoadFailed);
                if (this.hostEnvironment.IsDevelopment())
                {
                    return this.RedirectToAction(nameof(this.Error), new { message = ex.Message });
                }

                return this.RedirectToAction(nameof(this.Error));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message)
        {
            var errorModel = new ErrorViewModel
            {
                ErrorMessage = this.hostEnvironment.IsDevelopment() && !string.IsNullOrWhiteSpace(message)
                    ? message
                    : "Something went wrong. Please try again later.",
            };

            return this.View(errorModel);
        }

        #endregion
    }
}
