namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Tests.Mocking;

    [TestFixture]
    public class FavoriteServiceTest
    {
        private IFavoriteService favoriteService;
        private IList<int> favoriteSession;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            var userService = new SessionService(repository);

            favoriteService = new FavoriteService(repository);

            this.favoriteSession = new List<int> { 13, 14 };
        }

        [Test]
        public async Task FavoriteSessionReturnsTheCorrectData()
        {
            //Act
            ICollection<FavoriteExportModel> result = await this.favoriteService.GetSessionFavoriteAsync(this.favoriteSession);

            ICollection<FavoriteExportModel> expectedData = new List<FavoriteExportModel>()
            {
                new FavoriteExportModel
                {
                    Id = 13,
                    Name = "Product13",
                    Price = 130
                },
                new FavoriteExportModel
                {
                    Id = 14,
                    Name = "Product14",
                    Price = 140
                }
            };

            //Assert
            Assert.That(result, Is.EquivalentTo(expectedData).Using(new FavoriteExportModelComparer()));
        }

        [Test]
        public async Task FavoriteSessionReturnsEmptyCollection()
        {
            //Act
            ICollection<FavoriteExportModel> result = await this.favoriteService.GetSessionFavoriteAsync(new List<int>());

            //Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FavoriteSessionThrowsErrorIfTheProductsIsInvalid()
        {
            //Arrange
            ICollection<int> favoritesTooBig = new List<int>() { 20 };

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                ICollection<FavoriteExportModel> result = await this.favoriteService.GetSessionFavoriteAsync(favoritesTooBig);
            }, "The product does not exist.");

            ICollection<int> favoritesZero = new List<int>() { 0 };
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                ICollection<FavoriteExportModel> result = await this.favoriteService.GetSessionFavoriteAsync(favoritesZero);
            }, "The product does not exist.");

            ICollection<int> favoritesNegative = new List<int>() { -1 };
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                ICollection<FavoriteExportModel> result = await this.favoriteService.GetSessionFavoriteAsync(favoritesNegative);
            }, "The product does not exist.");
        }

        [Test]
        public async Task FavoriteDatabaseReturnsTheCorrectData()
        {
            //Arrange
            ICollection<FavoriteExportModel> expectedData = new List<FavoriteExportModel>()
            {
                new FavoriteExportModel
                {
                    Id = 13,
                    Name = "Product13",
                    Price = 130
                },
                new FavoriteExportModel
                {
                    Id = 14,
                    Name = "Product14",
                    Price = 140
                }
            };

            //Act
            ICollection<FavoriteExportModel> result = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer1");

            //Assert
            Assert.That(result, Is.EquivalentTo(expectedData).Using(new FavoriteExportModelComparer()));
        }

        [Test]
        public async Task FavoriteDatabaseReturnsEmptyFavorites()
        {
            //Act
            ICollection<FavoriteExportModel> result = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer2");

            //Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddToDatabaseAsyncThrowsExceptionIfTheUserIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.AddToDatabaseFavoriteAsync(2, "TestCustomer3");
            }, "The user does not exist.");
        }

        [Test]
        public void AddToDatabaseAsyncThrowsExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.AddToDatabaseFavoriteAsync(20, "TestCustomer2");
            }, "The user does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.AddToDatabaseFavoriteAsync(0, "TestCustomer2");
            }, "The user does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.AddToDatabaseFavoriteAsync(-1, "TestCustomer2");
            }, "The user does not exist.");
        }

        [Test]
        public async Task AddToDatabaseAsyncDontAddTheProductBecauseItIsAlreadyThere()
        {
            //Act
            await this.favoriteService.AddToDatabaseFavoriteAsync(13, "TestCustomer1");

            var favorites = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer1");

            //Assert
            Assert.That(favorites.Count == 2);
        }

        [Test]
        public async Task AddToDatabaseAsyncAddsTheProductCorrectlyToEmptyCollection()
        {
            //Act
            await this.favoriteService.AddToDatabaseFavoriteAsync(13, "TestCustomer2");

            var favorites = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer2");

            //Assert
            Assert.That(favorites.Count > 0);
            Assert.That(favorites.Any(f => f.Id == 13));
        }

        [Test]
        public async Task AddToDatabaseAsyncAddsTheProductCorrectlyToNotEmptyCollection()
        {
            //Act
            await this.favoriteService.AddToDatabaseFavoriteAsync(15, "TestCustomer1");

            var favorites = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer1");

            //Assert
            Assert.That(favorites.Any(f => f.Id == 15));
        }

        [Test]
        public void AddToSessionAsyncThrowsExceptionIfProductIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int> { 2, 4 };
                await this.favoriteService.AddToSessionFavoriteAsync(20, favorites);
            }, "The product does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int> { 2, 4 };
                await this.favoriteService.AddToSessionFavoriteAsync(0, favorites);
            }, "The product does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int> { 2, 4 };
                await this.favoriteService.AddToSessionFavoriteAsync(-1, favorites);
            }, "The product does not exist.");
        }

        [Test]
        public async Task AddToSessionAsyncReturnsTheSameCollectionBecauseTheProductIdIsAlreadyAdded()
        {
            //Act
            var favorites = await this.favoriteService.AddToSessionFavoriteAsync(3, this.favoriteSession);

            //Assert
            CollectionAssert.AreEqual(this.favoriteSession, favorites);
        }

        [Test]
        public async Task AddToSessionAsyncReturnsCollectionWithOneItemBecauseItWasEmpty()
        {
            //Act
            var favorites = await this.favoriteService.AddToSessionFavoriteAsync(3, new List<int>());

            //Assert
            Assert.That(favorites.Count == 1);
            Assert.That(favorites.Contains(3));
        }

        [Test]
        public async Task AddToSessionAsyncReturnsCollectionWithOneItemMoreBecauseItWasNotEmpty()
        {
            //Act
            var favorites = await this.favoriteService.AddToSessionFavoriteAsync(5, this.favoriteSession);

            //Assert
            Assert.That(favorites.Count == 3);
            Assert.That(favorites.Contains(5));
        }

        [Test]
        public async Task RemoveFromSessionAsyncShouldReturnTheSameCollectionIfTheProductIdIsNotInTheCollection()
        {
            //Act
            var favorites = await this.favoriteService.RemoveFromSessionFavoriteAsync(5, this.favoriteSession);

            //Assert
            CollectionAssert.AreEqual(this.favoriteSession, favorites);
        }

        [Test]
        public void RemoveFromSessionAsyncShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int> { 2, 4 };
                await this.favoriteService.RemoveFromSessionFavoriteAsync(20, favorites);
            }, "The product does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int> { 2, 4 };
                await this.favoriteService.RemoveFromSessionFavoriteAsync(0, favorites);
            }, "The product does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var favorites = new List<int> { 2, 4 };
                await this.favoriteService.RemoveFromSessionFavoriteAsync(-1, favorites);
            }, "The product does not exist.");
        }

        [Test]
        public async Task RemoveFromSessionAsyncShouldReturnEmptyCollectionIfTheCollectionAsParameterIsEmpty()
        {
            //Act
            var favorites = await this.favoriteService.RemoveFromSessionFavoriteAsync(5, new List<int>());

            //Assert
            CollectionAssert.AreEqual(new List<int>(), favorites);
        }

        [Test]
        public async Task RemoveFromSessionAsyncShouldRemoveTheProductIdCorrectly()
        {
            //Act
            var favorites = await this.favoriteService.RemoveFromSessionFavoriteAsync(13, this.favoriteSession);

            //Assert
            Assert.That(favorites.Count == 1);
            Assert.That(!favorites.Contains(13));
        }

        [Test]
        public void RemoveFromDatabaseAsyncShouldThrowExceptionIfTheUserIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.RemoveFromDatabaseFavoriteAsync(20, "TestCustomer3");
            }, "The user does not exist.");
        }

        [Test]
        public void RemoveFromDatabaseAsyncShouldThrowExceptionIfTheProductIsInvalid()
        {
            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.RemoveFromDatabaseFavoriteAsync(20, "TestCustomer2");
            }, "The product does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.RemoveFromDatabaseFavoriteAsync(0, "TestCustomer2");
            }, "The product does not exist.");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.favoriteService.RemoveFromDatabaseFavoriteAsync(-1, "TestCustomer2");
            }, "The product does not exist.");
        }

        [Test]
        public async Task RemoveFromDatabaseAsyncShouldNotRemoveAnythingIfTheProductIdIsNotPresentInTheDatabase()
        {
            //Act
            await this.favoriteService.RemoveFromDatabaseFavoriteAsync(15, "TestCustomer1");

            ICollection<FavoriteExportModel> favorites = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer1");

            ICollection<FavoriteExportModel> expectedData = new List<FavoriteExportModel>()
            {
                new FavoriteExportModel
                {
                    Id = 13,
                    Name = "Product13",
                    Price = 130
                },
                new FavoriteExportModel
                {
                    Id = 14,
                    Name = "Product14",
                    Price = 140
                }
            };

            //Assert
            Assert.That(favorites, Is.EquivalentTo(expectedData).Using(new FavoriteExportModelComparer()));
        }

        [Test]
        public async Task RemoveFromDatabaseAsyncShouldNotRemoveAnythintIfTheCustomerDoNotHaveAnyFavorites()
        {
            await this.favoriteService.RemoveFromDatabaseFavoriteAsync(15, "TestCustomer2");

            ICollection<FavoriteExportModel> favorites = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer2");

            ICollection<FavoriteExportModel> expectedData = new List<FavoriteExportModel>();

            //Assert
            Assert.That(favorites, Is.EquivalentTo(expectedData).Using(new FavoriteExportModelComparer()));
        }

        [Test]
        public async Task RemoveFromDatabaseAsyncShouldRemoveCorrectlyFromTheDatabase()
        {
            await this.favoriteService.RemoveFromDatabaseFavoriteAsync(13, "TestCustomer1");

            ICollection<FavoriteExportModel> favorites = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer1");

            ICollection<FavoriteExportModel> expectedData = new List<FavoriteExportModel>()
            {
                new FavoriteExportModel
                {
                    Id = 14,
                    Name = "Product14",
                    Price = 140
                }
            };

            //Assert
            Assert.That(favorites, Is.EquivalentTo(expectedData).Using(new FavoriteExportModelComparer()));
        }
    }
}
