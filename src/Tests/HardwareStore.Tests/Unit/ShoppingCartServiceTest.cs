namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.Services;
    using HardwareStore.Tests.Mocking;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using NUnit.Framework;

    [TestFixture]
    public class ShoppingCartServiceTest
    {
        private IShoppingCartService shoppingCartService = null!;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            this.shoppingCartService = new ShoppingCartService(repository);
        }

        #region GetDatabaseShoppingCartAsync

        [Test]
        public void GetDatabaseThrowsExceptionIfTheUserIdIsIncorrect()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var model = await this.shoppingCartService.GetDatabaseShoppingCartAsync("TestCustomer3");
            });
        }

        [Test]
        public async Task GetDatabaseReturnsTheEmptyIfTheUserDontHaveAnyFavorites()
        {
            //Act
            var model = await this.shoppingCartService.GetDatabaseShoppingCartAsync("TestCustomer2");

            //Assert
            Assert.That(model.TotalCartPrice == 0);
            Assert.That(model.Shoppings.Count == 0);
        }

        [Test]
        public async Task GetDatabaseShouldReturnTheCorrectData()
        {
            //Arrange
            var expectedResult = new ShoppingCartViewModel
            {
                Shoppings = new List<ShoppingCartItemViewModel>
                {
                    new ShoppingCartItemViewModel
                    {
                        ProductId = 13,
                        Quantity = 2,
                        Name = "Product13",
                        Price = 130,
                        TotalPrice = 260,
                        ProductQuantity = 13
                    },
                    new ShoppingCartItemViewModel
                    {
                        ProductId = 14,
                        Quantity = 3,
                        Name = "Product14",
                        Price = 140,
                        TotalPrice = 420,
                        ProductQuantity = 14
                    }
                },
                TotalCartPrice = 680
            };

            //Act 
            var model = await this.shoppingCartService.GetDatabaseShoppingCartAsync("TestCustomer1");

            //Assert
            Assert.That(model.TotalCartPrice == expectedResult.TotalCartPrice);

            Assert.That(model.Shoppings, Is.EqualTo(expectedResult.Shoppings).Using(new ShoppingCartItemViewModelComparer()));
        }

        #endregion

        #region AddToDatabaseShoppingCartAsync

        [Test]
        public void AddToDatabaseShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 2, "TestCustomer3");
            });
        }

        [Test]
        public void AddToDatabaseThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(20, 2, "TestCustomer1");
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(0, 2, "TestCustomer1");
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(-1, 2, "TestCustomer1");
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(99999, 2, "TestCustomer1");
            });
        }

        [Test]
        public void AddToDatabaseThrowsExceptionIfTheQuantityExceedsTheAvailableQuantityIfTheProductIsNotInTheCart()
        {
            //Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, 16, "TestCustomer1");
            });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, 20, "TestCustomer1");
            });
        }

        [Test]
        public async Task AddToDatabaseAddsOneItemToTheCustomerCartIfTheQuantityIsNegative()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, -5, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 15 && p.Quantity == 1));
        }

        [Test]
        public async Task AddToDatabaseAddsOneItemToTheCustomerCartIfTheQuantityIsMinusOne()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, -1, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 15 && p.Quantity == 1));
        }

        [Test]
        public async Task AddToDatabaseAddsOneItemToTheCustomerCartIfTheQuantityIsZero()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, -1, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 15 && p.Quantity == 1));
        }

        [Test]
        public async Task AddToDatabaseAddsOneItemToTheCustomerCartIfTheQuantityIsOne()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, 1, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 15 && p.Quantity == 1));
        }

        [Test]
        public async Task AddToDatabaseAddsOneItemToTheCustomerCartIfTheQuantityIsMoreThanFive()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, 5, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 15 && p.Quantity == 5));
        }

        [Test]
        public async Task AddToDatabaseAddsOneItemToTheCustomerCartIfTheQuantityIsEqualToTheAvailableQuantity()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(15, 15, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 15 && p.Quantity == 15));
        }

        [Test]
        public void AddToDatabaseThrowsExceptionIfTheProductIsPresentInTheDatabaseButTheResultQuantityExceedsTheProductQuantity()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 12, userId);
            });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 20, userId);
            });
        }

        [Test]
        public async Task AddToDatabaseAddOneToTheExistingItemIfTheQuantityIsNegative()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, -5, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 3));
        }

        [Test]
        public async Task AddToDatabaseAddOneToTheExistingItemIfTheQuantityIsMinusOne()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, -1, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 3));
        }

        [Test]
        public async Task AddToDatabaseAddOneToTheExistingItemIfTheQuantityIsZero()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 0, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 3));
        }

        [Test]
        public async Task AddToDatabaseAddOneToTheExistingItemIfTheQuantityIsOne()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 1, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 3));
        }

        [Test]
        public async Task AddToDatabaseAddOneToTheExistingItemIfTheQuantityIsMoreThanOne()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 5, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 7));
        }

        [Test]
        public async Task AddToDatabaseShouldAddItemToEmptyCollection()
        {
            //Arrange
            string userId = "TestCustomer2";

            //Act
            await this.shoppingCartService.AddToDatabaseShoppingCartAsync(13, 1, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Count == 1);
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 1));
        }

        #endregion

        #region IncreaseDatabaseItemQuantityAsync

        [Test]
        public void IncreaseQuantityDatabaseShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(20, userId);
            });
        }

        [Test]
        public void IncreaseQuantityDatabaseShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(20, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(0, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(-1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(16, userId);
            });
        }

        [Test]
        public async Task IncreaseQuantityDatabaseShouldIncreaseTheQuantityCorrectly()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 3));
        }

        [Test]
        public async Task IncreaseQuantityDatabaseShouldIncreaseTwiceTheQuantityCorrectly()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(13, userId);
            await this.shoppingCartService.IncreaseDatabaseItemQuantityAsync(13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(p => p.ProductId == 13 && p.Quantity == 4));
        }

        #endregion

        #region DecreaseDatabaseItemQuantityAsync

        [Test]
        public void DecreaseQuantityDatabaseShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(20, userId);
            });
        }

        [Test]
        public void DecreaseQuantityDatabaseShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(20, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(0, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(-1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(16, userId);
            });
        }

        [Test]
        public void DecreaseQuantityDatabaseShouldThrowExceptionIfTheProductIdIsNotInTheDatabase()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(7, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(15, userId);
            });
        }

        [Test]
        public async Task DecreaseQuantityDatabaseShouldDecreaseTheQuantityCorrectly()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(i => i.ProductId == 13 && i.Quantity == 1));
        }

        [Test]
        public async Task DecreaseQuantityDatabaseShouldDecreaseTheQuantityTwiceCorrectly()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(14, userId);
            await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(14, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(i => i.ProductId == 14 && i.Quantity == 1));
        }

        [Test]
        public async Task DecreaseQuantityDatabaseShouldDecreaseTheQuantityButIfTheResultQuantityIsBelowOneDontDecrease()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(13, userId);
            await this.shoppingCartService.DecreaseDatabaseItemQuantityAsync(13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(i => i.ProductId == 13 && i.Quantity == 1));
        }

        #endregion

        #region UpdateDatabaseItemQuantityAsync

        [Test]
        public void UpdateQuantityDatabaseShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 20, userId);
            });
        }

        [Test]
        public void UpdateQuantityDatabaseShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 20, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 0, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, -1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 16, userId);
            });
        }

        [Test]
        public void UpdateQuantityShouldWhrowExceptionIfTheProductIdIsNotInTheDatabaseCart()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 7, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 15, userId);
            });
        }

        [Test]
        public async Task UpdateQuantityDatabaseShouldUpdateTheQuantityOfTheItemWithFive()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(5, 13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(i => i.ProductId == 13 && i.Quantity == 5));
        }

        [Test]
        public async Task UpdateQuantityDatabaseShouldUpdateTheQuantityOfTheItemWithOne()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(1, 13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(i => i.ProductId == 13 && i.Quantity == 1));
        }

        [Test]
        public async Task UpdateQuantityDatabaseShouldUpdateTheQuantityOfTheItemWithThirteen()
        {
            //Arrange
            var userId = "TestCustomer1";

            //Act
            await this.shoppingCartService.UpdateDatabaseItemQuantityAsync(13, 13, userId);

            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(cart.Shoppings.Any(i => i.ProductId == 13 && i.Quantity == 13));
        }

        #endregion

        #region RemoveFromDatabaseShoppingCartAsync

        [Test]
        public void RemoveItemDatabaseShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(13, userId);
            });
        }

        [Test]
        public void RemoveItemDatabaseShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(20, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(0, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(-1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(16, userId);
            });
        }

        [Test]
        public void RemoveItemDatabaseShouldThrowExceptionIfTheProductIdIsNotInTheDatabase()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(1, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(7, userId);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(15, userId);
            });
        }

        [Test]
        public async Task RemoveItemDatabaseShouldRemoveTheItemCorrectly()
        {
            //Arrange
            string userId = "TestCustomer1";
            await this.shoppingCartService.RemoveFromDatabaseShoppingCartAsync(13, userId);

            //Act
            var cart = await this.shoppingCartService.GetDatabaseShoppingCartAsync(userId);

            //Assert
            Assert.That(!cart.Shoppings.Any(i => i.ProductId == 13));
        }

        #endregion
    }
}