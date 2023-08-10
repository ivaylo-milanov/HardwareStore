namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.Services;
    using HardwareStore.Tests.Mocking;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using System.ComponentModel.DataAnnotations;
    using HardwareStore.Infrastructure.Common;
    using Microsoft.EntityFrameworkCore;
    using HardwareStore.Infrastructure.Models;
    using Castle.Core.Resource;

    [TestFixture]
    public class SessionServiceTest
    {
        private ISessionService sessionService;
        private ICollection<int> favoritesSession;
        private ICollection<ShoppingCartExportModel> cartSession;
        private IRepository repository;

        [SetUp]
        public async Task Setup()
        {
            repository = await TestRepository.GetRepository();

            sessionService = new SessionService(repository);

            this.favoritesSession = new List<int> { 13, 14 };
            this.cartSession = new List<ShoppingCartExportModel>
            {
                new ShoppingCartExportModel
                {
                    ProductId = 13,
                    Quantity = 2
                },
                new ShoppingCartExportModel
                {
                    ProductId = 14,
                    Quantity = 3
                }
            };
        }

        [Test]
        public void AddToDatabaseShouldThrowAnExceptionIfTheUserIsIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.sessionService.AddToDatabase(userId, this.favoritesSession, this.cartSession);
            });
        }

        [Test]
        public async Task AddToDatabaseShouldNotAddAnythingBecauseTheSessionIsEmpty()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act
            await this.sessionService.AddToDatabase(userId, new List<int>(), new List<ShoppingCartExportModel>());

            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

            //Assert
            Assert.That(customer.Favorites.Count == 2);
            Assert.That(customer.ShoppingCartItems.Count == 2);
        }

        [Test]
        public async Task AddToDatabaseShouldNotAddAnythingIfAllProductsAreAlreadyInTheDatabase()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act
            await this.sessionService.AddToDatabase(userId, this.favoritesSession, this.cartSession);

            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

            //Assert
            Assert.That(customer.Favorites.Count == 2);
            Assert.That(customer.ShoppingCartItems.Count == 2);
        }

        [Test]
        public void AddToDatabaseShouldThrowExceptionIfTheProductIdInTheFavoriteSessionIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int>() { 20 };
                await this.sessionService.AddToDatabase(userId, favorites, this.cartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int>() { -1 };
                await this.sessionService.AddToDatabase(userId, favorites, this.cartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int>() { 0 };
                await this.sessionService.AddToDatabase(userId, favorites, this.cartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int>() { 16 };
                await this.sessionService.AddToDatabase(userId, favorites, this.cartSession);
            });
        }

        [Test]
        public void AddToDatabaseShouldThrowExceptionIfTheProductIdInTheCartSessionIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = new List<ShoppingCartExportModel>()
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = 20,
                        Quantity = 2
                    }
                };
                await this.sessionService.AddToDatabase(userId, this.favoritesSession, cart);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = new List<ShoppingCartExportModel>()
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = 0,
                        Quantity = 2
                    }
                };
                await this.sessionService.AddToDatabase(userId, this.favoritesSession, cart);
            });
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = new List<ShoppingCartExportModel>()
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = -1,
                        Quantity = 2
                    }
                };
                await this.sessionService.AddToDatabase(userId, this.favoritesSession, cart);
            });
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = new List<ShoppingCartExportModel>()
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = 16,
                        Quantity = 2
                    }
                };
                await this.sessionService.AddToDatabase(userId, this.favoritesSession, cart);
            });


        }

        [Test]
        public async Task AddToDatabaseShouldAddTheFavoritesCorrectly()
        {
            //Arrange
            var userId = "TestCustomer1";

            var favorites = new List<int>() { 2, 3 };

            //Act
            await this.sessionService.AddToDatabase(userId, favorites, this.cartSession);

            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

            //Assert
            Assert.That(customer.Favorites.Count == 4);
            Assert.That(customer.ShoppingCartItems.Count == 2);
            Assert.That(customer.Favorites.Any(f => f.ProductId == 2));
            Assert.That(customer.Favorites.Any(f => f.ProductId == 3));
        }

        [Test]
        public async Task AddToDatabaseShouldAddTheCartCorrectly()
        {
            //Arrange
            var userId = "TestCustomer1";

            var cart = new List<ShoppingCartExportModel>()
            {
                new ShoppingCartExportModel
                {
                    ProductId = 2,
                    Quantity = 3
                },
                new ShoppingCartExportModel
                {
                    ProductId = 3,
                    Quantity = 1
                }
            };

            //Act
            await this.sessionService.AddToDatabase(userId, this.favoritesSession, cart);

            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

            //Assert
            Assert.That(customer.Favorites.Count == 2);
            Assert.That(customer.ShoppingCartItems.Count == 4);
            Assert.That(customer.ShoppingCartItems.Any(f => f.ProductId == 2));
            Assert.That(customer.ShoppingCartItems.Any(f => f.ProductId == 3));
        }

        [Test]
        public async Task AddToDatabaseShouldUpdateTheQuantityOfTheProductsInTheCartIfTheyExistAlready()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act
            await this.sessionService.AddToDatabase(userId, this.favoritesSession, this.cartSession);

            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

            //Assert
            Assert.That(customer.Favorites.Count == 2);
            Assert.That(customer.ShoppingCartItems.Count == 2);
            Assert.That(customer.ShoppingCartItems.Any(c => c.ProductId == 13 && c.Quantity == 4));
            Assert.That(customer.ShoppingCartItems.Any(c => c.ProductId == 14 && c.Quantity == 6));
        }

        [Test]
        public async Task AddToDatabaseShouldAddTheFavoritesAndShoppingCartToTheEmptyCollectionsOfTheCustomer()
        {
            //Arrange
            var userId = "TestCustomer2";

            //Act
            await this.sessionService.AddToDatabase(userId, this.favoritesSession, this.cartSession);

            var customer = await this.repository
                .All<Customer>()
                .Include(c => c.Favorites)
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.Id == userId);

            //Assert
            Assert.That(customer.ShoppingCartItems.Count == 2);
            Assert.That(customer.ShoppingCartItems.Any(c => c.ProductId == 13 && c.Quantity == 2));
            Assert.That(customer.ShoppingCartItems.Any(c => c.ProductId == 14 && c.Quantity == 3));
            Assert.That(customer.Favorites.Any(c => c.ProductId == 14));
            Assert.That(customer.Favorites.Any(c => c.ProductId == 14));
        }
    }
}
