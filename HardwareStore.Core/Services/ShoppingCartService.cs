namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Core.Extensions;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingCartService(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToSessionShoppingCartAsync(int id, int quantity)
        {
            var shoppings = GetShoppingCart();
            var product = await this.repository.FindAsync<Product>(id);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var shoppingModel = shoppings.FirstOrDefault(p => p.ProductId == id);

            if (shoppingModel == null)
            {
                var model = new ShoppingCartExportModel
                {
                    ProductId = id,
                    Quantity = quantity,
                };

                shoppings.Add(model);
            }
            else
            {
                shoppingModel.Quantity += quantity;
            }

            SetShoppingCart(shoppings);
        }

        public async Task DecreaseSessionItemQuantityAsync(int productId)
        {
            var shoppings = GetShoppingCart();

            var product = await this.repository.FindAsync<Product>(productId);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var shoppingModel = shoppings.FirstOrDefault(p => p.ProductId == productId);

            if (shoppingModel != null && shoppingModel.Quantity > 1)
            {
                shoppingModel.Quantity--;
            }

            SetShoppingCart(shoppings);
        }

        public async Task<ShoppingCartViewModel> GetSessionShoppingCartAsync()
        {
            var shoppings = GetShoppingCart();
            var shoppingItems = new List<ShoppingCartItemViewModel>();

            foreach (var shopping in shoppings)
            {
                var product = await repository.FindAsync<Product>(shopping.ProductId);

                if (product == null)
                {
                    throw new ArgumentNullException("The product does not exist.");
                }

                ShoppingCartItemViewModel item = new ShoppingCartItemViewModel
                {
                    ProductId = shopping.ProductId,
                    Quantity = shopping.Quantity,
                    Name = product.Name,
                    Price = product.Price,
                    TotalPrice = product.Price * shopping.Quantity
                };

                shoppingItems.Add(item);
            }

            ShoppingCartViewModel model = new ShoppingCartViewModel
            {
                Shoppings = shoppingItems,
                TotalCartPrice = shoppingItems.Sum(x => x.TotalPrice)
            };

            return model;
        }

        public async Task RemoveFromSessionShoppingCartAsync(int id)
        {
            var shoppings = GetShoppingCart();
            var product = await this.repository.FindAsync<Product>(id);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var shoppingModel = shoppings.FirstOrDefault(p => p.ProductId == id);

            if (shoppingModel != null)
            {
                shoppings.Remove(shoppingModel);
            }

            SetShoppingCart(shoppings);
        }

        private void SetShoppingCart(ICollection<ShoppingCartExportModel> shoppings)
        {
            httpContextAccessor.HttpContext.Session.Set("Shopping Cart", shoppings);
        }

        private ICollection<ShoppingCartExportModel> GetShoppingCart()
            => httpContextAccessor.HttpContext.Session.Get<ICollection<ShoppingCartExportModel>>("Shopping Cart") ?? new List<ShoppingCartExportModel>();

        public async Task AddToDatabaseShoppingCartAsync(int productId, int quantity)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            var cartItem = user.ShoppingCartItems
            .FirstOrDefault(i => i.UserId == user.Id && i.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new ShoppingCartItem
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                user.ShoppingCartItems.Add(cartItem);
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task RemoveFromDatabaseShoppingCartAsync(int productId)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            var cartItem = user.ShoppingCartItems
                .FirstOrDefault(i => i.UserId == user.Id && i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            user.ShoppingCartItems.Remove(cartItem);

            await repository.SaveChangesAsync();
        }

        public async Task<ShoppingCartViewModel> GetDatabaseShoppingCartAsync()
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            var shoppings = user.ShoppingCartItems
                .Select(sci => new ShoppingCartItemViewModel
                {
                    ProductId = sci.Product.Id,
                    Name = sci.Product.Name,
                    Quantity = sci.Quantity,
                    Price = sci.Product.Price,
                    TotalPrice = sci.Product.Price * sci.Quantity
                })
                .ToList();

            var model = new ShoppingCartViewModel
            {
                Shoppings = shoppings,
                TotalCartPrice = shoppings.Sum(sci => sci.TotalPrice)
            };

            return model;
        }

        public async Task DecreaseDatabaseItemQuantityAsync(int productId)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            var cartItem = user.ShoppingCartItems
                .FirstOrDefault(i => i.UserId == user.Id && i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }

            await this.repository.SaveChangesAsync();
        }

        private async Task<Customer> GetUser(string userId)
            => await repository.AllReadonly<Customer>()
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

        private string GetUserId()
            => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
