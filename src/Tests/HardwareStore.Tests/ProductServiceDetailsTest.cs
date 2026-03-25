namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Tests.Mocking;

    [TestFixture]
    public class ProductServiceDetailsTest
    {
        private IProductService productService = null!;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();
            this.productService = new ProductService(repository);
        }

        [Test]
        public async Task TheProductDetialsReturnsDetailsAboutValidProduct()
        {
            var result = await this.productService.GetProductDetails(13);

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
                await this.productService.GetProductDetails(0);
            }, "The product does not exist.");
        }

        [Test]
        public void TheProductDetailsThrowsExceptionAboutInvalidProductWithProductIdMoreThanTheProductsCount()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.GetProductDetails(20);
            }, "The product does not exist.");
        }

        [Test]
        public void IsProductInDbFavoritesShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            const string userId = "TestCustomer3";

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.IsProductInDbFavorites(userId, 2);
            });
        }

        [Test]
        public void IsProductInDbFavoritesShouldThrowExceptionIfTheProductIdIsInvalid()
        {
            const string userId = "TestCustomer1";

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.IsProductInDbFavorites(userId, 20);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.IsProductInDbFavorites(userId, 0);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.IsProductInDbFavorites(userId, -1);
            });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.IsProductInDbFavorites(userId, 16);
            });
        }

        [Test]
        public async Task IsProductInDbFavoritesReturnTrueIfTheProductIdIsInTheFavorites()
        {
            const string userId = "TestCustomer1";

            var result = await this.productService.IsProductInDbFavorites(userId, 13);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsProductInDbFavoritesReturnFalseIfTheProductIdIsNotInTheFavorites()
        {
            const string userId = "TestCustomer1";

            var result = await this.productService.IsProductInDbFavorites(userId, 15);

            Assert.That(result, Is.False);
        }

    }
}
