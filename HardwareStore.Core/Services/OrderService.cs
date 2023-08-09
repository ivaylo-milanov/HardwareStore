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
            var customer = await GetOrdersCustomer(userId);

            var totalAmount = GetTotalAmount(customer.ShoppingCartItems);
            OrderFormModel model = new OrderFormModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.PhoneNumber,
                City = customer.City,
                Area = customer.Area,
                Address = customer.Address,
                TotalAmount = totalAmount
            };

            return model;
        }

        public async Task<IEnumerable<OrderViewModel>> GetUserOrders(string userId)
        {
            var customer = await GetOrdersCustomer(userId);

            var ordersModels = customer
                .Orders
                .Select(o => new OrderViewModel
                {
                    OrderId = o.Id.ToString(),
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                    Status = o.OrderStatus.ToString(),
                    TotalAmount = o.TotalAmount
                });

            return ordersModels;
        }

        public async Task OrderAsync(OrderFormModel model, string userId)
        {
            var customer = await GetOrdersCustomer(userId);

            var totalAmount = GetTotalAmount(customer.ShoppingCartItems);
            Order order = new Order
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
                OrderDate = DateTime.Now,
            };

            var orderProducts = new List<ProductOrder>();

            foreach (var item in customer.ShoppingCartItems)
            {
                var product = await this.repository.FindAsync<Product>(item.ProductId);

                if (product == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.ProductNotFound);
                }

                ProductOrder productOrder = new ProductOrder
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                orderProducts.Add(productOrder);

                product.Quantity -= item.Quantity;
            }

            order.ProductsOrders = orderProducts;
            await this.repository.AddAsync(order);

            this.repository.RemoveRange(customer.ShoppingCartItems);

            await this.repository.SaveChangesAsync();
        }

        private decimal GetTotalAmount(ICollection<ShoppingCartItem> cart) => cart.Sum(sc => sc.Quantity * sc.Product.Price);

        private async Task<Customer> GetOrdersCustomer(string userId)
        {
            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (customer == null)
            {
                throw new ArgumentNullException(ExceptionMessages.UserNotFound);
            }

            return customer;
        }
    }
}