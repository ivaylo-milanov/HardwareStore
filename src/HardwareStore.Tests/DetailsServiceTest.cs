namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Tests.Mocking;

    [TestFixture]
    public class DetailsServiceTest
    {
        private IDetailsService detailsService;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            detailsService = new DetailsService(repository);
        }

        [Test]
        public async Task TheProductDetialsReturnsDetailsAboutValidProduct()
        {
            //Arrange

            //Act
            var result = await detailsService.GetProductDetails(13);

            Assert.That(result.Id == 13);
            Assert.That(result.Name == "Product13");
            Assert.That(result.Manufacturer == null);
            Assert.That(result.Description == null);
            Assert.That(result.Warranty == 1);
            Assert.That(result.Attributes.Count() == 2);
            Assert.That(result.IsFavorite == false);
            Assert.That(result.Price == 130);
        }

        [Test]
        public void TheProductDetailsThrowsExceptionAboutInvalidProduct()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await detailsService.GetProductDetails(0);
            }, "The product does not exist.");
        }

        [Test]
        public void TheProductDetailsThrowsExceptionAboutInvalidProductWithProductIdMoreThanTheProductsCount()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await detailsService.GetProductDetails(20);
            }, "The product does not exist.");
        }

        [Test]
        public void IsProductInDbFavoritesShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInDbFavorites(userId, 2);
            });
        }

        [Test]
        public void IsProductInDbFavoritesShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInDbFavorites(userId, 20);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInDbFavorites(userId, 0);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInDbFavorites(userId, -1);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInDbFavorites(userId, 16);
            });
        }

        [Test]
        public async Task IsProductInDbFavoritesReturnTrueIfTheProductIdIsInTheFavorites()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            var result = await this.detailsService.IsProductInDbFavorites(userId, 13);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsProductInDbFavoritesReturnFalseIfTheProductIdIsNotInTheFavorites()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            var result = await this.detailsService.IsProductInDbFavorites(userId, 15);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsProductInSessionFavoritesShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            //Arrange
            var favorites = new List<int>() { 13, 14 };

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInSessionFavorites(favorites, 20);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInSessionFavorites(favorites, 0);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInSessionFavorites(favorites, -1);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await this.detailsService.IsProductInSessionFavorites(favorites, 16);
            });
        }

        [Test]
        public async Task IsProductInSessionFavoritesReturnTrueIfTheProductIdIsInTheFavorites()
        {
            //Arrange
            var favorites = new List<int>() { 13, 14 };

            //Act
            var result = await this.detailsService.IsProductInSessionFavorites(favorites, 13);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsProductInSessionFavoritesReturnFalseIfTheProductIdIsNotInTheFavorites()
        {
            //Arrange
            var favorites = new List<int>() { 13, 14 };

            //Act
            var result = await this.detailsService.IsProductInSessionFavorites(favorites, 15);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}
