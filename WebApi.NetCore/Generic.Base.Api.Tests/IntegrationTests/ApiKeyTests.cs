namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Test for the api key middleware.
    /// </summary>
    public class ApiKeyTests
    {
        [Fact]
        public async Task GetShouldFailWithInvalidApiKey()
        {
            var client = TestFactory.GetClient();
            client.DefaultRequestHeaders.Add(
                "x-api-key",
                "api key");

            var response = await client.GetAsync("api/ApiKeyTest");

            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }

        [Fact]
        public async Task GetShouldFailWithoutApiKey()
        {
            var client = TestFactory.GetClient();

            var response = await client.GetAsync("api/ApiKeyTest");

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }

        [Fact]
        public async Task GetShouldShouldWithMatchingApiKey()
        {
            var client = TestFactory.GetClientWithApiKey();

            var response = await client.GetAsync("api/ApiKeyTest");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
    }
}
