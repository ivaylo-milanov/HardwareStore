namespace HardwareStore.Components
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class FavoriteViewComponent : ViewComponent
    {
        private readonly IProductService productService;
        private readonly ILogger<FavoriteViewComponent> logger;

        public FavoriteViewComponent(IProductService productService, ILogger<FavoriteViewComponent> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            DetailsFavoriteModel model = new DetailsFavoriteModel
            {
                IsFavorite = false,
                ProductId = productId
            };
            try
            {
                var isFavorite = await this.productService.IsProductInSessionFavorites(GetFavorites(), productId);

                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    isFavorite = await this.productService.IsProductInDbFavorites(HttpContext.User.GetUserId(), productId);
                }

                model.IsFavorite = isFavorite;
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogError(ex, ex.Message);
            }

            return View(model);
        }

        private ICollection<int> GetFavorites()
            => HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();
    }
}
