namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IProductService productService, ILogger<HomeController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<HomeViewModel>> Get()
        {
            try
            {
                return this.Ok(await this.productService.GetHomeViewModelAsync());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, LogMessages.HomePageLoadFailed);
                return this.Problem(statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
