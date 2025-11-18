namespace HardwareStore.Components
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class FavoriteViewComponent : ViewComponent
    {
        private readonly IDetailsService detailsService;
        private readonly ILogger<FavoriteViewComponent> logger;

        public FavoriteViewComponent(IDetailsService detailsService, ILogger<FavoriteViewComponent> logger)
        {
            this.detailsService = detailsService;
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
                var isFavorite = await this.detailsService.IsProductInSessionFavorites(GetFavorites(), productId);

                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    isFavorite = await this.detailsService.IsProductInDbFavorites(HttpContext.User.GetUserId(), productId);
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
