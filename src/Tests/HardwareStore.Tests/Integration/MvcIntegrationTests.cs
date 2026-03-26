namespace HardwareStore.Tests.Integration
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc.Testing;

    [TestFixture]
    public class MvcIntegrationTests
    {
        private HardwareStoreWebApplicationFactory factory = null!;

        #region Fixture lifecycle

        [SetUp]
        public void SetUp()
        {
            this.factory = new HardwareStoreWebApplicationFactory();
        }

        [TearDown]
        public void TearDown()
        {
            this.factory.Dispose();
        }

        #endregion

        #region Tests

        [Test]
        public async Task HomeIndex_ReturnsSuccess()
        {
            var client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false,
            });

            var response = await client.GetAsync("/");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CartIndex_Unauthenticated_RedirectsToLogin()
        {
            var client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false,
            });

            var response = await client.GetAsync("/Cart");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
            Assert.That(response.Headers.Location?.PathAndQuery, Does.Contain("Login"));
        }

        [Test]
        public async Task CheckoutIndex_Unauthenticated_RedirectsToLogin()
        {
            var client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false,
            });

            var response = await client.GetAsync("/Checkout");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
            Assert.That(response.Headers.Location?.PathAndQuery, Does.Contain("Login"));
        }

        [Test]
        public async Task SearchFilterSearch_PostWithoutAntiforgery_ReturnsBadRequest()
        {
            var client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false,
            });

            var response = await client.PostAsync("/Search/FilterSearch", content: null);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion
    }
}
