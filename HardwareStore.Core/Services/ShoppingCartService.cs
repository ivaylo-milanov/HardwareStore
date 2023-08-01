namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Extensions;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingCartService(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToSessionShoppingCartAsync(int productId, int quantity)
        {
            var shoppings = GetShoppingCart();

            var product = await this.repository.FindAsync<Product>(productId);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            if (quantity > product.Quantity)
            {
                throw new InvalidOperationException($"Only {product.Quantity} \"{product.Name}\" left in stock.");
            }

            var cartItem = shoppings.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                var model = new ShoppingCartExportModel
                {
                    ProductId = productId,
                    Quantity = quantity,
                };

                shoppings.Add(model);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            SetShoppingCart(shoppings);
        }

        public async Task DecreaseSessionItemQuantityAsync(int productId)
        {
            var shoppings = GetShoppingCart();

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = shoppings.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
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
                    TotalPrice = product.Price * shopping.Quantity,
                    ProductQuantity = product.Quantity
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

        public async Task RemoveFromSessionShoppingCartAsync(int productId)
        {
            var shoppings = GetShoppingCart();

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException("The product does not exist.");
            }

            var cartItem = shoppings.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            shoppings.Remove(cartItem);

            SetShoppingCart(shoppings);
        }

        public async Task IncreaseSessionItemQuantityAsync(int productId)
        {
            var shoppings = GetShoppingCart();

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = shoppings.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            cartItem.Quantity++;

            SetShoppingCart(shoppings);
        }

        public async Task UpdateSessionItemQuantityAsync(ShoppingCartUpdateModel model)
        {
            var shoppings = GetShoppingCart();

            if (!await this.repository.AnyAsync<Product>(p => p.Id == model.ProductId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = shoppings.FirstOrDefault(p => p.ProductId == model.ProductId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            if (cartItem.Quantity == 1)
            {
                await this.RemoveFromSessionShoppingCartAsync(model.ProductId);
            }
            else
            {
                if (cartItem.Quantity < 0)
                {
                    cartItem.Quantity = 1;
                }
                else
                {
                    cartItem.Quantity = model.Quantity;
                }

                SetShoppingCart(shoppings);
            }
        }

        public async Task AddToDatabaseShoppingCartAsync(int productId, int quantity)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            var product = await this.repository.FindAsync<Product>(productId);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            if (quantity > product.Quantity)
            {
                throw new InvalidOperationException($"Only {product.Quantity} \"{product.Name}\" left in stock.");
            }

            var cartItem = user.ShoppingCartItems.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                var model = new ShoppingCartItem
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Quantity = quantity,
                };

                await this.repository.AddAsync(model);
            }
            else
            {
                cartItem.Quantity += quantity;
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

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = user.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            this.repository.Remove(cartItem);

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

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = user.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == productId);

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

        public async Task IncreaseDatabaseItemQuantityAsync(int productId)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = user.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            cartItem.Quantity++;

            await this.repository.SaveChangesAsync();
        }

        private void SetShoppingCart(ICollection<ShoppingCartExportModel> shoppings)
            => httpContextAccessor.HttpContext.Session.Set("Shopping Cart", shoppings);

        private ICollection<ShoppingCartExportModel> GetShoppingCart()
            => httpContextAccessor.HttpContext.Session.Get<ICollection<ShoppingCartExportModel>>("Shopping Cart") ?? new List<ShoppingCartExportModel>();

        private async Task<Customer> GetUser(string userId)
            => await repository.All<Customer>()
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == userId);

        private string GetUserId()
            => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task UpdateDatabaseItemQuantityAsync(ShoppingCartUpdateModel model)
        {
            var user = await GetUser(GetUserId());

            if (user == null)
            {
                throw new ArgumentNullException("User is not logged in.");
            }

            if (!await this.repository.AnyAsync<Product>(p => p.Id == model.ProductId))
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var cartItem = user.ShoppingCartItems
               .FirstOrDefault(i => i.ProductId == model.ProductId);

            if (cartItem == null)
            {
                throw new ArgumentNullException("The cart item does not exist.");
            }

            if (model.Quantity == 0)
            {
                await this.RemoveFromDatabaseShoppingCartAsync(model.ProductId);
            }
            else
            {
                if (model.Quantity < 0)
                {
                    cartItem.Quantity = 1;
                }
                else
                {
                    cartItem.Quantity = model.Quantity;
                }

                await this.repository.SaveChangesAsync();
            }
        }
    }
}