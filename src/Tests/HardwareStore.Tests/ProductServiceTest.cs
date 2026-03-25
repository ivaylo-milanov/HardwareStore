namespace HardwareStore.Tests
{
    using HardwareStore.Core.Enums;
    using HardwareStore.Core.Services;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Tests.Mocking;
    using Microsoft.EntityFrameworkCore;

    [TestFixture]
    public class ProductServiceTest
    {
        private ProductService productService = null!;

        [SetUp]
        public async Task SetUp()
        {
            var repository = await TestRepository.GetRepository();
            this.productService = new ProductService(repository);
        }

        [Test]
        public async Task CategoryExists_ReturnsTrue_ForSeededCategory()
        {
            var exists = await this.productService.CategoryExistsAsync("Category1");
            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task CategoryExists_ReturnsFalse_ForUnknownCategory()
        {
            var exists = await this.productService.CategoryExistsAsync("NotARealCategory");
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task GetCategoryCatalog_ReturnsProductsWithOptions()
        {
            var model = await this.productService.GetCategoryCatalogAsync("Category1");

            Assert.That(model.Products, Is.Not.Empty);
            Assert.That(model.Filters.Any(f => f.Name == "Manufacturer"), Is.True);
            var first = model.Products.First();
            Assert.That(first.Options, Is.Not.Empty);
        }

        [Test]
        public async Task GetSearchCatalog_WithEmptyKeyword_ReturnsProducts()
        {
            var model = await this.productService.GetSearchCatalogAsync(string.Empty);
            Assert.That(model.Products, Is.Not.Empty);
        }

        [Test]
        public async Task FilterCategoryCatalog_ByManufacturer_ReducesResults()
        {
            var catalog = await this.productService.GetCategoryCatalogAsync("Category1");
            var mfr = catalog.Products.First().Manufacturer;
            var filterJson = $"{{\"Order\":1,\"Manufacturer\":[\"{mfr}\"]}}";

            var filtered = await this.productService.FilterCategoryCatalogAsync("Category1", filterJson);
            Assert.That(filtered.All(p => p.Manufacturer == mfr), Is.True);
        }

        [Test]
        public async Task FilterCategoryCatalog_OrdersByLowestPrice()
        {
            var filterJson = $"{{\"Order\":{(int)ProductOrdering.LowestPrice}}}";
            var filtered = (await this.productService.FilterCategoryCatalogAsync("Category1", filterJson)).ToList();
            Assert.That(filtered.Count, Is.GreaterThan(1));
            Assert.That(filtered[0].Price, Is.LessThanOrEqualTo(filtered[1].Price));
        }
    }
}
