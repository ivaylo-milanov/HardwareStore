namespace HardwareStore.Tests
{
    using HardwareStore.Common;
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

        #region GetProductDetails

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
            Assert.That(result.AssemblyComponents.Count, Is.EqualTo(0));
            Assert.That(result.IsFavorite == false);
            Assert.That(result.Price == 130);
        }

        [Test]
        public void TheProductDetailsThrowsExceptionAboutInvalidProduct()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.GetProductDetails(0);
            }, ExceptionMessages.ProductNotFound);
        }

        [Test]
        public void TheProductDetailsThrowsExceptionAboutInvalidProductWithProductIdMoreThanTheProductsCount()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await this.productService.GetProductDetails(999);
            }, ExceptionMessages.ProductNotFound);
        }

        [Test]
        public async Task GetProductDetails_ReturnsAssemblyComponents_OrderedBySortOrder()
        {
            var result = await this.productService.GetProductDetails(16);

            Assert.That(result.AssemblyComponents.Count, Is.EqualTo(2));
            Assert.That(result.AssemblyComponents[0].Role, Is.EqualTo("CPU"));
            Assert.That(result.AssemblyComponents[0].ProductId, Is.EqualTo(1));
            Assert.That(result.AssemblyComponents[0].Name, Is.EqualTo("Product1"));
            Assert.That(result.AssemblyComponents[0].Quantity, Is.EqualTo(1));
            Assert.That(result.AssemblyComponents[1].Role, Is.EqualTo("GPU"));
            Assert.That(result.AssemblyComponents[1].ProductId, Is.EqualTo(2));
        }

        #endregion

        #region IsProductInDbFavorites

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
                await this.productService.IsProductInDbFavorites(userId, 999);
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

        #endregion
    }
}
