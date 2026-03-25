namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository repository;

        public ShoppingCartService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddToDatabaseShoppingCartAsync(int productId, int quantity, string userId)
        {
            var customer = await GetCartCustomer(userId);

            if (quantity <= 0) quantity = 1;

            var product = await this.repository.FindAsync<Product>(productId);

            if (product == null)
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.ShoppingCartItems.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                if (quantity > product.Quantity)
                {
                    throw new InvalidOperationException(String.Format(ExceptionMessages.NotManyItemsLeftInStock, product.Quantity, product.Name));
                }

                var model = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };

                customer.ShoppingCartItems.Add(model);
            }
            else
            {
                if (cartItem.Quantity + quantity > product.Quantity)
                {
                    throw new InvalidOperationException(String.Format(ExceptionMessages.NotManyItemsLeftInStock, product.Quantity, product.Name));
                }

                cartItem.Quantity += quantity;
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task RemoveFromDatabaseShoppingCartAsync(int productId, string userId)
        {
            var customer = await GetCartCustomer(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            this.repository.Remove(cartItem);
            await repository.SaveChangesAsync();
        }

        public async Task<ShoppingCartViewModel> GetDatabaseShoppingCartAsync(string userId)
        {
            var customer = await GetCartCustomer(userId);

            var shoppings = customer.ShoppingCartItems
                .Select(sci => new ShoppingCartItemViewModel
                {
                    ProductId = sci.Product.Id,
                    Name = sci.Product.Name,
                    Quantity = sci.Quantity,
                    Price = sci.Product.Price,
                    TotalPrice = sci.Product.Price * sci.Quantity,
                    ProductQuantity = sci.Product.Quantity
                })
                .ToList();

            var model = new ShoppingCartViewModel
            {
                Shoppings = shoppings,
                TotalCartPrice = shoppings.Sum(sci => sci.TotalPrice)
            };

            return model;
        }

        public async Task DecreaseDatabaseItemQuantityAsync(int productId, string userId)
        {
            var customer = await GetCartCustomer(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task IncreaseDatabaseItemQuantityAsync(int productId, string userId)
        {
            var customer = await GetCartCustomer(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            cartItem.Quantity++;

            await this.repository.SaveChangesAsync();
        }

        public async Task UpdateDatabaseItemQuantityAsync(int quantity, int productId, string userId)
        {
            var customer = await GetCartCustomer(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = customer.ShoppingCartItems.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            cartItem.Quantity = quantity;
            await this.repository.SaveChangesAsync();
        }

        private async Task<Customer> GetCartCustomer(string userId)
        {
            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.ShoppingCartItems)
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
