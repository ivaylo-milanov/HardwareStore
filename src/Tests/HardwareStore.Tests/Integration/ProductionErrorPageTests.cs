namespace HardwareStore.Tests.Integration
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc.Testing;

    [TestFixture]
    public class ProductionErrorPageTests
    {
        [Test]
        public async Task Error_InProduction_DoesNotEchoQueryMessage()
        {
            const string injected = "ExceptionDetailsShouldNotAppearInProduction";

            await using var factory = new HardwareStoreWebApplicationFactory("Production");

            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            var response = await client.GetAsync("/Home/Error?message=" + Uri.EscapeDataString(injected));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var body = await response.Content.ReadAsStringAsync();
            Assert.That(body, Does.Not.Contain(injected));
            Assert.That(body, Does.Contain("Something went wrong"));
        }
    }
}
