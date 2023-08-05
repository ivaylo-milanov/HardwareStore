namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly IRepository repository;

        public OrderService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<OrderFormModel> GetOrderModel(string userId)
        {
            var user = await this.repository
                .All<Customer>()
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(sc => sc.Product)
                .FirstOrDefaultAsync(p => p.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            var totalAmount = user.ShoppingCartItems.Sum(sc => sc.Quantity * sc.Product.Price);

            OrderFormModel model = new OrderFormModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                City = user.City,
                Area = user.Area,
                Address = user.Address,
                TotalAmount = totalAmount
            };

            return model;
        }

        public async Task<IEnumerable<OrderViewModel>> GetUserOrders(string userId)
        {
            var user = await this.repository
                .All<Customer>()
                .Include(p => p.Orders)
                .ThenInclude(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            var orders = user
                .Orders
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId.ToString(),
                    OrderDate = o.Order.OrderDate.ToString("yyyy-MM-dd"),
                    Status = o.Order.OrderStatus.ToString(),
                    TotalAmount = o.Order.TotalAmount
                });

            return orders;
        }

        public async Task OrderAsync(OrderFormModel model, string userId)
        {
            Order order = new Order
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                City = model.City,
                Area = model.Area,
                Address = model.Address,
                AdditionalNotes = model.AdditionalNotes,
                TotalAmount = model.TotalAmount,
                CustomerId = userId,
                OrderStatus = OrderStatus.Pending,
                PaymentMethod = model.PaymentMethod,
                OrderDate = DateTime.Now,
            };

            var user = await this.repository
                .All<Customer>()
                .Include(p => p.ShoppingCartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            var orderItems = user.ShoppingCartItems;
            var orderProducts = new List<ProductOrder>();

            foreach (var item in orderItems)
            {
                if (!await this.repository.AnyAsync<Product>(p => p.Id == item.ProductId))
                {
                    throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
                }

                ProductOrder productOrder = new ProductOrder
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                order.ProductsOrders = orderProducts;
            }

            await this.repository.AddAsync(order);
            await this.repository.SaveChangesAsync();
        }
    }
}
