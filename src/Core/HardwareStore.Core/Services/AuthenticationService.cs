namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;
        private readonly IRepository repository;

        public AuthenticationService(
            SignInManager<Customer> signInManager,
            UserManager<Customer> userManager,
            IRepository repository)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.repository = repository;
        }

        public async Task<SignInResult> LoginAsync(LoginFormModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.AccountNotFound);
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

            return result;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterFormModel model)
        {
            Customer user = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                City = model.City,
                Area = model.Area,
                Address = model.Address
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task MergeSessionCartAndFavoritesAsync(string userId, ICollection<int> favorites, ICollection<ShoppingCartExportModel> cart)
        {
            if (!await this.repository.AnyAsync<Customer>(c => c.Id == userId))
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            if (favorites != null && favorites.Count > 0)
            {
                await this.AddSessionFavoritesToDatabaseAsync(userId, favorites);
            }

            if (cart != null && cart.Count > 0)
            {
                await this.AddSessionShoppingCartToDatabaseAsync(userId, cart);
            }

            await this.repository.SaveChangesAsync();
        }

        private async Task AddSessionFavoritesToDatabaseAsync(string userId, ICollection<int> favorites)
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
                    dbFavorites.Add(new Favorite
                    {
                        ProductId = productId,
                        CustomerId = userId
                    });
                }
            }

            this.repository.AddRange(dbFavorites);
        }

        private async Task AddSessionShoppingCartToDatabaseAsync(string userId, ICollection<ShoppingCartExportModel> cart)
        {
            ICollection<ShoppingCartItem> newItems = new List<ShoppingCartItem>();
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
                    newItems.Add(new ShoppingCartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        CustomerId = userId
                    });
                }
                else
                {
                    existingCartItem.Quantity += item.Quantity;
                }
            }

            this.repository.AddRange(newItems);
        }
    }
}
