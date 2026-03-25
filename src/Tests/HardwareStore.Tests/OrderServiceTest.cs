namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using HardwareStore.Tests.Mocking;
    using Microsoft.EntityFrameworkCore;

    public class OrderServiceTest
    {
        private IOrderService orderService = null!;
        private IRepository repository = null!;

        [SetUp]
        public async Task Setup()
        {
            this.repository = await TestRepository.GetRepository();

            this.orderService = new OrderService(this.repository);
        }

        [Test]
        public void GetOrderModelShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            const string userId = "TestCustomer3";

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.orderService.GetOrderModel(userId);
            });
        }

        [Test]
        public async Task GetOrderModelShouldReturnTheCorrectData()
        {
            const string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.FirstName, Is.EqualTo("FirstName1"));
            Assert.That(model.LastName, Is.EqualTo("LastName1"));
            Assert.That(model.Phone, Is.EqualTo("Phone1"));
            Assert.That(model.Area, Is.EqualTo("Area1"));
            Assert.That(model.City, Is.EqualTo("City1"));
            Assert.That(model.Address, Is.EqualTo("Address1"));
            Assert.That(model.AdditionalNotes, Is.Null);
            Assert.That(model.TotalAmount, Is.EqualTo(680));
            Assert.That(model.PaymentMethod, Is.EqualTo(PaymentMethod.CreditCard));
        }

        [Test]
        public void GetUsersOrdersShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            const string userId = "TestCustomer3";

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.orderService.GetUserOrders(userId);
            });
        }

        [Test]
        public async Task GetUsersOrdersShouldReturnTheCorrectData()
        {
            const string userId = "TestCustomer1";

            var orders = await this.orderService.GetUserOrders(userId);

            Assert.That(orders, Is.Not.Null);
            Assert.That(orders.Count(), Is.EqualTo(1));
        }

        [Test]
        public void OrderAsyncShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            const string userId = "TestCustomer3";

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var order = new OrderFormModel();
                await this.orderService.OrderAsync(order, userId);
            });
        }

        [Test]
        public async Task OrderAsyncShouldMakeTheOrder()
        {
            const string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            await this.orderService.OrderAsync(model, userId);

            var orders = await this.orderService.GetUserOrders(userId);
            var newOrder = orders.Single(o => o.TotalAmount == 680m);

            var persisted = await this.repository
                .All<Order>()
                .FirstAsync(o => o.Id == newOrder.OrderId);

            Assert.That(persisted.OrderDate.Date, Is.EqualTo(DateTime.UtcNow.Date));
            Assert.That(newOrder.Status, Is.EqualTo(OrderStatus.Pending));
            Assert.That(newOrder.TotalAmount, Is.EqualTo(680));
        }

        [Test]
        public async Task OrderAsyncShouldAddTheProductOrders()
        {
            const string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            await this.orderService.OrderAsync(model, userId);

            var orders = await this.orderService.GetUserOrders(userId);
            var newOrder = orders.Single(o => o.TotalAmount == 680m);

            var productOrders = this.repository.All<ProductOrder>(o => o.OrderId == newOrder.OrderId).ToList();

            Assert.That(productOrders.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task OrderAsyncShouldLowerTheQuantityOfTheProducts()
        {
            const string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            await this.orderService.OrderAsync(model, userId);

            var quantity1 = (await this.repository.FindAsync<Product>(13))!.Quantity;
            var quantity2 = (await this.repository.FindAsync<Product>(14))!.Quantity;

            Assert.That(quantity1, Is.EqualTo(11));
            Assert.That(quantity2, Is.EqualTo(11));
        }

        [Test]
        public void OrderAsyncShouldThrowWhenShoppingCartIsEmpty()
        {
            const string userId = "TestCustomer2";

            var model = new OrderFormModel
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                Phone = "Phone2",
                City = "City2",
                Area = "Area2",
                Address = "Address2",
                PaymentMethod = PaymentMethod.CreditCard
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.orderService.OrderAsync(model, userId);
            });
        }

        [Test]
        public void OrderAsyncShouldThrowWhenInsufficientStock()
        {
            const string userId = "TestCustomer1";

            var cartItems = this.repository.All<ShoppingCartItem>().Where(i => i.CustomerId == userId).ToList();
            this.repository.RemoveRange(cartItems);
            this.repository.AddRange(new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    CustomerId = userId,
                    ProductId = 13,
                    Quantity = 999
                }
            });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.repository.SaveChangesAsync();
                var model = await this.orderService.GetOrderModel(userId);
                await this.orderService.OrderAsync(model, userId);
            });
        }

        [Test]
        public async Task OrderAsyncUsesServerCalculatedTotalNotPostedValue()
        {
            const string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);
            model.TotalAmount = 1m;

            await this.orderService.OrderAsync(model, userId);

            var orders = await this.orderService.GetUserOrders(userId);
            var newOrder = orders.Single(o => o.TotalAmount == 680m);

            Assert.That(newOrder.TotalAmount, Is.EqualTo(680));
        }
    }
}
