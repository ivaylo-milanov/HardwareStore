namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class FavoriteService : IFavoriteService
    {
        private readonly IRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FavoriteService(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToDatabaseFavoriteAsync(int productId)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = user.Favorites.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                Favorite favorite = new Favorite
                {
                    ProductId = productId,
                    UserId = user.Id
                };

                user.Favorites.Add(favorite);
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task AddToSessionFavoriteAsync(int productId)
        {
            var favorites = GetFavorites();

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            if (!favorites.Contains(productId))
            {
                favorites.Add(productId);
                SetFavorites(favorites);
            }
        }

        public async Task RemoveFromDatabaseFavoriteAsync(int productId)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = user.Favorites.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            this.repository.Remove(cartItem);
            await this.repository.SaveChangesAsync();
        }

        public async Task RemoveFromSessionFavoriteAsync(int productId)
        {
            var favorites = GetFavorites();

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }
            
            if (!favorites.Contains(productId))
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            favorites.Remove(productId);
            SetFavorites(favorites);
        }

        public async Task<ICollection<FavoriteExportModel>> GetDatabaseFavoriteAsync()
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            var favorites = user
                .Favorites
                .Select(f => new FavoriteExportModel
                {
                    Id = f.ProductId,
                    Name = f.Product.Name,
                    Price = f.Product.Price
                })
                .ToList();

            return favorites;
        }

        public async Task<ICollection<FavoriteExportModel>> GetSessionFavoriteAsync()
        {
            var favorites = GetFavorites();
            var favoriteItems = new List<FavoriteExportModel>();

            foreach (var favoriteId in favorites)
            {
                var product = await this.repository.FindAsync<Product>(favoriteId);

                if (product == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
                }

                var favoriteItem = new FavoriteExportModel
                {
                    Id = favoriteId,
                    Name = product.Name,
                    Price = product.Price
                };

                favoriteItems.Add(favoriteItem);
            }

            return favoriteItems;
        }

        public async Task<bool> IsFavorite(int productId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                var favorites = GetFavorites();
                return favorites.Contains(productId);
            }

            var user = await GetUser(userId);
            return user.Favorites.Any(f => f.ProductId == productId);
        }

        private void SetFavorites(ICollection<int> shoppings)
            => httpContextAccessor.HttpContext.Session.Set("Favorite", shoppings);

        private ICollection<int> GetFavorites()
            => httpContextAccessor.HttpContext.Session.Get<ICollection<int>>("Favorite") ?? new List<int>();

        private async Task<Customer> GetUser(string userId)
            => await repository.All<Customer>()
                .Include(c => c.Favorites)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == userId);

        private string GetUserId()
            => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
