namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Profile;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly IRepository repository;

        public UserService(IRepository repository)
        {
            this.repository = repository;
        }

        private async Task AddToFavoritesAsync(string userId, ICollection<int> favorites)
        {
            ICollection<Favorite> dbFavorites = new List<Favorite>();
            foreach (var productId in favorites)
            {
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
            await this.AddToFavoritesAsync(userId, favorites);
            await this.AddToShoppingCartAsync(userId, cart);

            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<Favorite>> GetCustomerFavorites(string userId)
        {
            var customer = await GetCustomerWithFavorites(userId); 

            return customer.Favorites;
        }

        public async Task<ICollection<ShoppingCartItem>> GetCustomerShoppingCart(string userId)
        {
            var customer = await GetCustomerWithShoppingCart(userId);

            return customer.ShoppingCartItems;
        }

        public async Task<ProfileViewModel> GetCustomerProfile(string userId)
        {
            var customer = await this.repository.FindAsync<Customer>(userId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            ProfileViewModel model = new ProfileViewModel
            {
                FullName = customer.FirstName + ' ' + customer.LastName,
                City = customer.City,
                Address = customer.Address,
                Email = customer.Email
            };

            return model;
        }

        public async Task<Customer> GetCustomerWithShoppingCart(string userId)
        {
            var customer = await repository.All<Customer>()
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            return customer;
        }

        public async Task<Customer> GetCustomerWithOrders(string userId)
        {
            var customer = await this.repository
                .All<Customer>()
                .Include(p => p.Orders)
                .ThenInclude(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == userId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            return customer;
        }

        public async Task<Customer> GetCustomerWithFavorites(string userId)
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