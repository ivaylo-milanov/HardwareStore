namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FavoriteService : IFavoriteService
    {
        private readonly IRepository repository;

        public FavoriteService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddToDatabaseFavoriteAsync(int productId, string userId)
        {
            var customer = await GetFavoritesCustomer(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.Favorites.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                Favorite favorite = new Favorite
                {
                    ProductId = productId
                };

                customer.Favorites.Add(favorite);
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task<ICollection<int>> AddToSessionFavoriteAsync(int productId, ICollection<int> favorites)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            if (!favorites.Contains(productId))
            {
                favorites.Add(productId);
            }

            return favorites;
        }

        public async Task RemoveFromDatabaseFavoriteAsync(int productId, string userId)
        {
            var customer = await GetFavoritesCustomer(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.Favorites.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem != null)
            {
                this.repository.Remove(cartItem);
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task<ICollection<int>> RemoveFromSessionFavoriteAsync(int productId, ICollection<int> favorites)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }
            
            if (favorites.Contains(productId))
            {
                favorites.Remove(productId);
            }

            return favorites;
        }

        public async Task<ICollection<FavoriteExportModel>> GetDatabaseFavoriteAsync(string userId)
        {
            var customer = await GetFavoritesCustomer(userId);
            ICollection<FavoriteExportModel> favoriteItems = new List<FavoriteExportModel>();

            var favoritesModels = customer
                .Favorites
                .Select(f => new FavoriteExportModel
                {
                    Id = f.ProductId,
                    Name = f.Product.Name,
                    Price = f.Product.Price
                })
                .ToList();

            return favoritesModels;
        }

        public async Task<ICollection<FavoriteExportModel>> GetSessionFavoriteAsync(ICollection<int> favorites)
        {
            ICollection<FavoriteExportModel> favoriteItems = new List<FavoriteExportModel>();

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

        private async Task<Customer> GetFavoritesCustomer(string userId)
        {
            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            return customer;
        }
    }
}