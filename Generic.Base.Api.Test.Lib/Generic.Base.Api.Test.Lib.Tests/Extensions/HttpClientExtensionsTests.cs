namespace Generic.Base.Api.Test.Lib.Tests.Extensions
{
    using Generic.Base.Api.Tests.Lib.Extensions;

    /// <summary>
    ///     Tests for <see cref="HttpClientExtensions" />.
    /// </summary>
    public class HttpClientExtensionsTests
    {
        [Fact]
        public void AddApiKey()
        {
            const string apiKey = nameof(apiKey);

            using var client = new HttpClient();
            client.AddApiKey(apiKey);

            Assert.Equal(
                apiKey,
                client.DefaultRequestHeaders.GetValues("x-api-key").FirstOrDefault());
        }
    }
}
