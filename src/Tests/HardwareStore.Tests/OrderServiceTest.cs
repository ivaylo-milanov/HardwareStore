namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.Services;
    using HardwareStore.Tests.Mocking;
    using HardwareStore.Core.ViewModels.Order;
    using HardwareStore.Infrastructure.Models.Enums;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;

    public class OrderServiceTest
    {
        private IOrderService orderService;
        private IRepository repository;

        [SetUp]
        public async Task Setup()
        {
            repository = await TestRepository.GetRepository();

            orderService = new OrderService(repository);
        }

        [Test]
        public void GetOrderModelShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer3";

            //Act and Arrange
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var model = await orderService.GetOrderModel(userId);
            });
        }

        [Test]
        public async Task GetOrderModelShouldReturnTheCorrectData()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            var model = await this.orderService.GetOrderModel(userId);

            //Assert
            Assert.That(model != null);
            Assert.That(model.FirstName == "FirstName1");
            Assert.That(model.LastName == "LastName1");
            Assert.That(model.Phone == "Phone1");
            Assert.That(model.Area == "Area1");
            Assert.That(model.City == "City1");
            Assert.That(model.Address == "Address1");
            Assert.That(model.AdditionalNotes == null);
            Assert.That(model.TotalAmount == 680);
            Assert.That(model.PaymentMethod == 0);
        }

        [Test]
        public void GetUsersOrdersShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer3";

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var model = await orderService.GetUserOrders(userId);
            });
        }

        [Test]
        public async Task GetUsersOrdersShouldReturnTheCorrectData()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            var orders = await this.orderService.GetUserOrders(userId);

            Assert.That(orders != null);
            Assert.That(orders.Count() == 1);
        }

        [Test]
        public void OrderAsyncShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer3";

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var order = new OrderFormModel();
                await orderService.OrderAsync(order, userId);
            });
        }

        [Test]
        public async Task OrderAsyncShouldMakeTheOrder()
        {
            //Arrange
            string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            //Act
            await this.orderService.OrderAsync(model, userId);

            var orders = await this.orderService.GetUserOrders(userId);
            var newOrder = orders.OrderBy(o => o.OrderDate).Last();

            //Assert
            Assert.That(newOrder.OrderDate == DateTime.Now.ToString("yyyy-MM-dd"));
            Assert.That(newOrder.Status == OrderStatus.Pending.ToString());
            Assert.That(newOrder.TotalAmount == 680);
        }

        [Test]
        public async Task OrderAsyncShouldAddTheProductOrders()
        {
            //Arrange
            string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            //Act
            await this.orderService.OrderAsync(model, userId);

            var orders = await this.orderService.GetUserOrders(userId);
            var newOrder = orders.OrderBy(o => o.OrderDate).Last();

            List<ProductOrder> productOrders = this.repository.All<ProductOrder>(o => o.OrderId.ToString() == newOrder.OrderId).ToList();

            //Assert
            Assert.That(productOrders.Count() == 2);
        }

        [Test]
        public async Task OrderAsyncShouldLowerTheQuantityOfTheProducts()
        {
            //Arrange
            string userId = "TestCustomer1";

            var model = await this.orderService.GetOrderModel(userId);

            //Act
            await this.orderService.OrderAsync(model, userId);


            var quantity1 = (await this.repository.FindAsync<Product>(13)).Quantity;
            var quantity2 = (await this.repository.FindAsync<Product>(14)).Quantity;

            Assert.That(quantity1 == 11);
            Assert.That(quantity2 == 11);
        }
    }
}