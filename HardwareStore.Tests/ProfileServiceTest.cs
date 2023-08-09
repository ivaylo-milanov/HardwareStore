namespace HardwareStore.Tests
{
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Tests.Mocking;
    using NUnit.Framework;

    [TestFixture]
    public class ProfileServiceTest
    {
        private IProfileService profileService;

        [SetUp]
        public async Task Setup()
        {
            var repository = await TestRepository.GetRepository();

            profileService = new ProfileService(repository);
        }

        [Test]
        public void GetProfileShouldThrowExceptionIfTheUserIdIsInvalid()
        {
            //Arrange
            string userId = "TestCustomer3";

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var customer = await this.profileService.GetProfileModel(userId);
            });
        }

        [Test]
        public async Task GetProfileShouldReturnTheCorrectData()
        {
            //Arrange
            string userId = "TestCustomer1";

            //Act
            var profile = await this.profileService.GetProfileModel(userId);

            //Assert
            Assert.That(profile.FullName == "FirstName1 LastName1");
            Assert.That(profile.Email == "customer1@mail.com");
            Assert.That(profile.Address == "Address1");
            Assert.That(profile.City == "City1");
        }
    }
}
