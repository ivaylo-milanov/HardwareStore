namespace HardwareStore.Core.Services
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly IRepository repository;
        private readonly IUserService userService;

        public OrderService(IRepository repository, IUserService userService)
        {
            this.repository = repository;
            this.userService = userService;
        }

        public async Task<OrderFormModel> GetOrderModel(string userId)
        {
            var customer = await this.userService.GetCustomerWithShoppingCart(userId);

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
            var customer = await userService.GetCustomerWithShoppingCart(userId);

            var orders = customer
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
            var cart = await this.userService.GetCustomerShoppingCart(userId);

            var totalAmount = GetTotalAmount(cart);
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

            foreach (var item in cart)
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

                orderProducts.Add(productOrder);
            }

            order.ProductsOrders = orderProducts;
            await this.repository.AddAsync(order);

            this.repository.RemoveRange(cart);

            await this.repository.SaveChangesAsync();
        }

        private decimal GetTotalAmount(ICollection<ShoppingCartItem> cart) => cart.Sum(sc => sc.Quantity * sc.Product.Price);
    }
}