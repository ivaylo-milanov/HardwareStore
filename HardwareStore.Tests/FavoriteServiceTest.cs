namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.Favorite;
    using HardwareStore.Tests.Mocking;
    using Moq;

    [TestFixture]
    public class FavoriteServiceTest
    {
        private IFavoriteService favoriteService;
        private IList<int> favoriteSession;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            var userService = new Mock<IUserService>();

            favoriteService = new FavoriteService(repository, userService.Object);

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
        public async Task FavoriteSessionThrowsErrorIfTheProductsIsInvalid()
        {
            //Arrange
            ICollection<int> favorites = new List<int>() { 20 };

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                ICollection<FavoriteExportModel> result = await this.favoriteService.GetSessionFavoriteAsync(favorites);
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
            ICollection<FavoriteExportModel> result = await this.favoriteService.GetDatabaseFavoriteAsync("TestCustomer");

            //Assert
            Assert.That(result, Is.EquivalentTo(expectedData).Using(new FavoriteExportModelComparer()));
        }
    }
}
