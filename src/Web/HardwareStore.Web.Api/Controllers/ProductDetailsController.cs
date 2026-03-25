namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Web.Api.Extensions;
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/products")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductDetailsController> logger;

        public ProductDetailsController(IProductService productService, ILogger<ProductDetailsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet("{productId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDetailsModel>> Get(int productId)
        {
            ProductDetailsModel model;
            try
            {
                model = await this.productService.GetProductDetails(productId);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, LogMessages.ProductRequestFailed);
                return this.NotFound();
            }

            model.IsFavorite = false;
            if (this.User.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    model.IsFavorite = await this.productService.IsProductInDbFavorites(this.User.GetUserId(), productId);
                }
                catch (ArgumentNullException ex)
                {
                    this.logger.LogError(ex, LogMessages.ProductRequestFailed);
                }
            }

            return this.Ok(model);
        }
    }
}
