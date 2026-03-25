namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Tests.Mocking;

    [TestFixture]
    public class FavoriteServiceTest
    {
        private IFavoriteService favoriteService = null!;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            this.favoriteService = new FavoriteService(repository);
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
