namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;
        private readonly IRepository repository;

        public UserService(SignInManager<Customer> signInManager, UserManager<Customer> userManager, IRepository repository)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.repository = repository;
        }

        public async Task<SignInResult> LoginAsync(LoginFormModel model, ICollection<ShoppingCartExportModel> cart, ICollection<int> favorites)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.AccountNotFound);
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                if (cart != null)
                {
                    await AddToCartAsync(user.Id, cart);
                }

                if (favorites != null)
                {
                    await AddToFavoritesAsync(user.Id, favorites);
                }
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterFormModel model, ICollection<ShoppingCartExportModel> cart, ICollection<int> favorites)
        {
            Customer user = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                City = model.City,
                Area = model.Area,
                Address = model.Address
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                if (cart != null)
                {
                    await AddToCartAsync(user.Id, cart);
                }

                if (favorites != null)
                {
                    await AddToFavoritesAsync(user.Id, favorites);
                }
            }

            return result;
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
            await this.repository.SaveChangesAsync();
        }

        private async Task AddToCartAsync(string userId, ICollection<ShoppingCartExportModel> cart)
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
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<Favorite>> GetCustomerFavorites(string userId)
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

            return customer.Favorites;
        }
    }
}
