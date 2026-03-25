namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class OrderService : IOrderService
    {
        private readonly IRepository repository;

        public OrderService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<OrderFormModel> GetOrderModel(string userId)
        {
            var customer = await this.GetCustomerForCheckoutAsync(userId);

            var totalAmount = this.GetTotalAmount(customer.ShoppingCartItems);
            return new OrderFormModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.PhoneNumber,
                City = customer.City,
                Area = customer.Area,
                Address = customer.Address,
                TotalAmount = totalAmount,
                PaymentMethod = PaymentMethod.CreditCard
            };
        }

        public async Task<IEnumerable<OrderViewModel>> GetUserOrders(string userId)
        {
            if (!await this.repository.AnyAsync<Customer>(c => c.Id == userId))
            {
                throw new InvalidOperationException(ExceptionMessages.UserNotFound);
            }

            return await this.repository
                .AllReadonly<Order>()
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.OrderStatus,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();
        }

        public async Task OrderAsync(OrderFormModel model, string userId)
        {
            await using IDbContextTransaction transaction = await this.repository.BeginTransactionAsync();
            try
            {
                var customer = await this.GetCustomerForCheckoutAsync(userId);
                var cartItems = customer.ShoppingCartItems.ToList();
                if (cartItems.Count == 0)
                {
                    throw new InvalidOperationException(ExceptionMessages.EmptyShoppingCart);
                }

                var totalAmount = this.GetTotalAmount(cartItems);
                var orderProducts = new List<ProductOrder>();

                foreach (var item in cartItems)
                {
                    var product = await this.repository.FindAsync<Product>(item.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException(ExceptionMessages.ProductNotFound);
                    }

                    if (item.Quantity > product.Quantity)
                    {
                        throw new InvalidOperationException(
                            string.Format(ExceptionMessages.NotManyItemsLeftInStock, product.Quantity, product.Name));
                    }

                    orderProducts.Add(new ProductOrder
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });

                    product.Quantity -= item.Quantity;
                }

                var order = new Order
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    City = model.City,
                    Area = model.Area,
                    Address = model.Address,
                    AdditionalNotes = model.AdditionalNotes,
                    TotalAmount = totalAmount,
                    CustomerId = userId,
                    OrderStatus = OrderStatus.Pending,
                    PaymentMethod = model.PaymentMethod,
                    OrderDate = DateTime.UtcNow,
                    ProductsOrders = orderProducts
                };

                await this.repository.AddAsync(order);
                this.repository.RemoveRange(cartItems);
                await this.repository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private decimal GetTotalAmount(ICollection<ShoppingCartItem> cart) =>
            cart.Sum(sc => sc.Quantity * sc.Product.Price);

        private async Task<Customer> GetCustomerForCheckoutAsync(string userId)
        {
            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(sc => sc.Product)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (customer == null)
            {
                throw new InvalidOperationException(ExceptionMessages.UserNotFound);
            }

            return customer;
        }
    }
}
