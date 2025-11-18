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
        private IShoppingCartService shoppingCartService;
        private IList<ShoppingCartExportModel> shoppingCartSession;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            shoppingCartService = new ShoppingCartService(repository);

            this.shoppingCartSession = new List<ShoppingCartExportModel>
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
        public void GetSessionShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var shoppingCart = new List<ShoppingCartExportModel>
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = 20,
                        Quantity = 2
                    }
                };

                await this.shoppingCartService.GetSessionShoppingCartAsync(shoppingCart);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var shoppingCart = new List<ShoppingCartExportModel>
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = 0,
                        Quantity = 2
                    }
                };

                await this.shoppingCartService.GetSessionShoppingCartAsync(shoppingCart);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var shoppingCart = new List<ShoppingCartExportModel>
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = -1,
                        Quantity = 2
                    }
                };

                await this.shoppingCartService.GetSessionShoppingCartAsync(shoppingCart);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var shoppingCart = new List<ShoppingCartExportModel>
                {
                    new ShoppingCartExportModel
                    {
                        ProductId = 16,
                        Quantity = 2
                    }
                };

                await this.shoppingCartService.GetSessionShoppingCartAsync(shoppingCart);
            });
        }

        [Test]
        public async Task GetSessionShouldReturTheCorrectData()
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
            var model = await this.shoppingCartService.GetSessionShoppingCartAsync(this.shoppingCartSession);

            //Assert
            Assert.That(model.TotalCartPrice == expectedResult.TotalCartPrice);

            Assert.That(model.Shoppings, Is.EqualTo(expectedResult.Shoppings).Using(new ShoppingCartItemViewModelComparer()));
        }

        [Test]
        public async Task GetSessionReturnsEmptyShoppingCart()
        {
            //Act
            var model = await this.shoppingCartService.GetSessionShoppingCartAsync(new List<ShoppingCartExportModel>());

            //Assert
            Assert.That(model.TotalCartPrice == 0);
            Assert.That(model.Shoppings.Count == 0);
        }

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

        [Test]
        public void AddToSessionThrowsExceptionIfTheProductIdIsIncorrect()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(20, 1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(0, 1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(-1, 1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(16, 1, this.shoppingCartSession);
            });
        }

        [Test]
        public async Task AddToSessionAddsTheProductEvenThoughTheQuantityIsZero()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, 0, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 15);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 1);
        }

        [Test]
        public async Task AddToSessionAddsTheProductEvenThoughTheQuantityIsOne()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, 1, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 15);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 1);
        }

        [Test]
        public async Task AddToSessionAddsTheProductWithQuantityMoreThanZeroO()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, 5, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 15);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 5);
        }

        [Test]
        public async Task AddToSessionAddsTheProductEvenThoughTheQuantityIsMinusOne()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, -1, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 15);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 1);
        }

        [Test]
        public async Task AddToSessionAddsTheProductEvenThoughTheQuantityIsNegative()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, -5, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 15);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 1);
        }

        [Test]
        public async Task AddToSessionAddsTheProductIfTheQuantityIsEqualToTheAvailableQuantity()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, 15, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 15);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 15);
        }

        [Test]
        public void AddToSessionShouldExceptionIfTheQuantityExceedsTheAvailableQuantity()
        {
            //Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, 16, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(15, 20, this.shoppingCartSession);
            });
        }

        [Test]
        public void AddToSessionShouldThrowExceptionIfTheProductIsInTheCartAndTheQuantityExceedsTheAvailableQuantity()
        {
            //Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 12, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 20, this.shoppingCartSession);
            });
        }

        [Test]
        public async Task AddToSessionShouldAddOneToTheQuantityOfTheExistingItemButTheQuantityIsZero()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 0, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 13);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 3);
        }

        [Test]
        public async Task AddToSessionShouldAddOneToTheQuantityOfTheExistingItemButTHeQuantityIsMinusOne()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, -1, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 13);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 3);
        }

        [Test]
        public async Task AddToSessionShouldAddOneToTheQuantityOfTheExistingItemButTHeQuantityIsNegative()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, -5, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 13);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 3);
        }

        [Test]
        public async Task AddToSessionShouldAddOneToTheQuantityOfTheExistingItemButTheQuantityIsOne()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 1, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 13);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 3);
        }

        [Test]
        public async Task AddToSessionShouldAddOneToTheQuantityOfTheExistingItemButTheQuantityIsFive()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 5, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 13);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 7);
        }

        [Test]
        public async Task AddToSessionShouldAddOneToTheQuantityOfTheExistingItemButTheResultQuantityIsEqualToTheProductQuantity()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 11, this.shoppingCartSession);

            var item = cart.FirstOrDefault(i => i.ProductId == 13);

            //Assert
            Assert.That(item != null);
            Assert.That(item.Quantity == 13);
        }

        [Test]
        public async Task AddToSessionShouldAddItemToEmptySession()
        {
            //Act
            var cart = await this.shoppingCartService.AddToSessionShoppingCartAsync(13, 5, new List<ShoppingCartExportModel>());
            var item = cart.First();

            //Assert
            Assert.That(cart.Count == 1);

            Assert.That(item.ProductId == 13);
            Assert.That(item.Quantity == 5);
        }

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
                await this.shoppingCartService.AddToDatabaseShoppingCartAsync(16, 2, "TestCustomer1");
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

        [Test]
        public void IncreaseQuantitySessionShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(20, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(0, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(-1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(16, this.shoppingCartSession);
            });
        }

        [Test]
        public void IncreaseQuantitySessionShouldThrowExceptionIfTheProductIdIsNotInTheSession()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(7, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(15, this.shoppingCartSession);
            });
        }

        [Test]
        public async Task IncreaseQuantitySessionShouldIncreaseCorrectlyTheQuantity()
        {
            //Act
            var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(p => p.ProductId == 13 && p.Quantity == 3));
        }

        [Test]
        public async Task IncreaseQuantitySessionShouldIncreaseTwiceTheQuantity()
        {
            //Act
            var cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(13, this.shoppingCartSession);
            cart = await this.shoppingCartService.IncreaseSessionItemQuantityAsync(13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(p => p.ProductId == 13 && p.Quantity == 4));
        }

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

        [Test]
        public void DecreaseQuantitySessionShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(20, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(0, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(-1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(16, this.shoppingCartSession);
            });
        }

        [Test]
        public void DecreaseQuantitySessionShouldThrowExceptionIfTheProductIdIsNotInTheSession()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(7, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.shoppingCartService.DecreaseSessionItemQuantityAsync(15, this.shoppingCartSession);
            });
        }

        [Test]
        public async Task DecreaseQuantitySessionShouldDecreaseTheQuantityCorrectly()
        {
            //Act
            var cart = await this.shoppingCartService.DecreaseSessionItemQuantityAsync(13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(i => i.ProductId == 13 && i.Quantity == 1));
        }

        [Test]
        public async Task DecreaseQuantitySessionShouldDecreaseTheQuantityTwiceCorrectly()
        {
            //Act
            var cart = await this.shoppingCartService.DecreaseSessionItemQuantityAsync(14, this.shoppingCartSession);
            cart = await this.shoppingCartService.DecreaseSessionItemQuantityAsync(14, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(i => i.ProductId == 14 && i.Quantity == 1));
        }

        [Test]
        public async Task DecreaseQuantitySessionShouldDecreaseTheQuantityButIfTheResultQuantityIsBelowOneDontDecrease()
        {
            //Act
            var cart = await this.shoppingCartService.DecreaseSessionItemQuantityAsync(13, this.shoppingCartSession);
            cart = await this.shoppingCartService.DecreaseSessionItemQuantityAsync(13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(i => i.ProductId == 13 && i.Quantity == 1));
        }

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

        [Test]
        public void UpdateQuantitySessionShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 20, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 0, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, -1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 16, this.shoppingCartSession);
            });
        }

        [Test]
        public void UpdateQuantitySessionShouldThrowExceptionIfTheProductIdIsNotInTheSession()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 7, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 15, this.shoppingCartSession);
            });
        }

        [Test]
        public async Task UpdateQuantitySessionShouldUpdateTheQuantityOfTheItemWithFive()
        {
            //Act
            var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(5, 13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(i => i.ProductId == 13 && i.Quantity == 5));
        }

        [Test]
        public async Task UpdateQuantitySessionShouldUpdateTheQuantityOfTheItemWithOne()
        {
            //Act
            var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(1, 13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(i => i.ProductId == 13 && i.Quantity == 1));
        }

        [Test]
        public async Task UpdateQuantitySessionShouldUpdateTheQuantityOfTheItemWithThirteen()
        {
            //Act
            var cart = await this.shoppingCartService.UpdateSessionItemQuantityAsync(13, 13, this.shoppingCartSession);

            //Assert
            Assert.That(cart.Any(i => i.ProductId == 13 && i.Quantity == 13));
        }

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

        [Test]
        public void RemoveItemSessionShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(20, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(0, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(-1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(16, this.shoppingCartSession);
            });
        }

        [Test]
        public void RemoveItemSessionShouldThrowExceptionIfTheProductIdIsNotInTheSession()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(1, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(7, this.shoppingCartSession);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(15, this.shoppingCartSession);
            });
        }

        [Test]
        public async Task RemoveItemSessionShouldRemoveTheItemSuccessfully()
        {
            var cart = await this.shoppingCartService.RemoveFromSessionShoppingCartAsync(13, this.shoppingCartSession);

            Assert.That(!cart.Any(i => i.ProductId == 13));
        }

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
    }
}