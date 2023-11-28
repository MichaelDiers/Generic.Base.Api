namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net.Http.Json;
    using Generic.Base.Api.Extensions;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Tests for <see cref="HttpContextExtensions" />.
    /// </summary>
    [Trait(
        "TestType",
        "InMemoryIntegrationTest")]
    public class HttpContextExtensionsTest
    {
        /// <see cref="SetResponseController" />
        [Fact]
        public async Task SetResponseTest()
        {
            var client = TestFactory.GetClient().AddApiKey();

            const int status = 500;
            const string message = "message";

            var response = await client.GetAsync($"/api/SetResponse/{status}/{message}");

            Assert.Equal(
                status,
                (int) response.StatusCode);
            Assert.Equal(
                "application/json; charset=utf-8",
                response.Content.Headers.ContentType?.ToString());

            var errorResult = await response.Content.ReadFromJsonAsync<ErrorResult>();
            Assert.NotNull(errorResult);
            Assert.Equal(
                message,
                errorResult.Error);
        }
    }
}
