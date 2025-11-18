namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.Services;
    using HardwareStore.Tests.Mocking;

    public class HomeServiceTest
    {
        private IHomeService homeService;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            homeService = new HomeService(repository);
        }

        [Test]
        public async Task GetHomeModelShouldReturnTheCorrectData()
        {
            //Act
            var model = await this.homeService.GetHomeModel();

            //Assert
            Assert.That(model.NewestProducts.Count() == 4);
            Assert.That(model.MostBoughtProducts.Count() == 2);
        }
    }
}
