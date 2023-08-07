namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository repository;
        private readonly IUserService userService;

        public ShoppingCartService(IRepository repository, IUserService userService)
        {
            this.repository = repository;
            this.userService = userService;
        }

        public async Task<ICollection<ShoppingCartExportModel>> AddToSessionShoppingCartAsync(int productId, int quantity, ICollection<ShoppingCartExportModel> cart)
        {
            var product = await this.repository.FindAsync<Product>(productId);

            if (product == null)
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                var model = new ShoppingCartExportModel
                {
                    ProductId = productId,
                    Quantity = quantity,
                };

                cart.Add(model);
            }
            else
            {
                if (cartItem.Quantity + quantity > product.Quantity)
                {
                    throw new InvalidOperationException(String.Format(ExceptionMessages.NotManyItemsLeftInStock, product.Quantity, product.Name));
                }

                cartItem.Quantity += quantity;
            }

            return cart;
        }

        public async Task<ICollection<ShoppingCartExportModel>> DecreaseSessionItemQuantityAsync(int productId, ICollection<ShoppingCartExportModel> cart)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }

            return cart;
        }

        public async Task<ShoppingCartViewModel> GetSessionShoppingCartAsync(ICollection<ShoppingCartExportModel> cart)
        {
            var shoppingItems = new List<ShoppingCartItemViewModel>();

            foreach (var shopping in cart)
            {
                var product = await repository.FindAsync<Product>(shopping.ProductId);

                if (product == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
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

        public async Task<ICollection<ShoppingCartExportModel>> RemoveFromSessionShoppingCartAsync(int productId, ICollection<ShoppingCartExportModel> cart)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            cart.Remove(cartItem);

            return cart;
        }

        public async Task<ICollection<ShoppingCartExportModel>> IncreaseSessionItemQuantityAsync(int productId, ICollection<ShoppingCartExportModel> cart)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            cartItem.Quantity++;

            return cart;
        }

        public async Task<ICollection<ShoppingCartExportModel>> UpdateSessionItemQuantityAsync(int quantity, int productId, ICollection<ShoppingCartExportModel> cart)
        {
            if (!await this.repository.AnyAsync<Product>(p => p.Id == quantity))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductId == quantity);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            if (cartItem.Quantity == 1)
            {
                await this.RemoveFromSessionShoppingCartAsync(quantity, cart);
            }
            else
            {
                if (cartItem.Quantity < 0)
                {
                    cartItem.Quantity = 1;
                }
                else
                {
                    cartItem.Quantity = quantity;
                }
            }

            return cart;
        }

        public async Task AddToDatabaseShoppingCartAsync(int productId, int quantity, string userId)
        {
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            var product = await this.repository.FindAsync<Product>(productId);

            if (product == null)
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem == null)
            {
                var model = new ShoppingCartItem
                {
                    CustomerId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                };

                await this.repository.AddAsync(model);
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
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart
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
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            var shoppings = cart
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
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart
                .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task IncreaseDatabaseItemQuantityAsync(int productId, string userId)
        {
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart
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
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            if (!await this.repository.AnyAsync<Product>(p => p.Id == productId))
            {
                throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
            }

            var cartItem = cart
               .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new ArgumentNullException(ExceptionMessages.CartItemNotFound);
            }

            if (quantity == 0)
            {
                await this.RemoveFromDatabaseShoppingCartAsync(productId, userId);
            }
            else
            {
                if (quantity < 0)
                {
                    cartItem.Quantity = 1;
                }
                else
                {
                    cartItem.Quantity = quantity;
                }

                await this.repository.SaveChangesAsync();
            }
        }
    }
}