namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using System.Threading.Tasks;

    public class SessionService : ISessionService
    {
        private readonly IRepository repository;

        public SessionService(IRepository repository)
        {
            this.repository = repository;
        }
        private async Task AddToFavoritesAsync(string userId, ICollection<int> favorites)
        {
            ICollection<Favorite> dbFavorites = new List<Favorite>();
            foreach (var productId in favorites)
            {
                if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
                {
                    throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
                }

                var existingFavorite = await this.repository
                    .FirstOrDefaultAsync<Favorite>(f => f.ProductId == productId && f.CustomerId == userId);

                if (existingFavorite == null)
                {
                    var favoriteProduct = new Favorite
                    {
                        ProductId = productId,
                        CustomerId = userId
                    };

                    dbFavorites.Add(favoriteProduct);
                }
            }

            this.repository.AddRange(dbFavorites);
        }

        private async Task AddToShoppingCartAsync(string userId, ICollection<ShoppingCartExportModel> cart)
        {
            ICollection<ShoppingCartItem> shoppings = new List<ShoppingCartItem>();
            foreach (var item in cart)
            {
                if (!await this.repository.AnyAsync<Product>(p => p.Id == item.ProductId))
                {
                    throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
                }

                var existingCartItem = await this.repository
                    .FirstOrDefaultAsync<ShoppingCartItem>(f => f.ProductId == item.ProductId && f.CustomerId == userId);

                if (existingCartItem == null)
                {
                    var dbCartItem = new ShoppingCartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        CustomerId = userId
                    };

                    shoppings.Add(dbCartItem);
                }
                else
                {
                    existingCartItem.Quantity += item.Quantity;
                }
            }

            this.repository.AddRange(shoppings);
        }

        public async Task AddToDatabase(string userId, ICollection<int> favorites, ICollection<ShoppingCartExportModel> cart)
        {
            if (!await this.repository.AnyAsync<Customer>(c => c.Id == userId))
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            if (favorites != null && favorites.Count > 0)
            {
                await this.AddToFavoritesAsync(userId, favorites);
            }

            if (cart != null && cart.Count > 0)
            {
                await this.AddToShoppingCartAsync(userId, cart);
            }

            await this.repository.SaveChangesAsync();
        } 
    }
}