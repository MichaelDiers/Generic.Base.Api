namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net.Http.Json;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Tests for <see cref="OptionsControllerBase" />.
    /// </summary>
    public class OptionsControllerTest
    {
        [Fact]
        public async Task OptionsLinksShouldBeAccessibleAnonymous()
        {
            var client = TestFactory.GetClient().AddApiKey();

            var response = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    "/api/Options"));

            response.EnsureSuccessStatusCode();
            var linkResult = await response.Content.ReadFromJsonAsync<ClientLinkResult>();
            Assert.NotNull(linkResult);
            Assert.NotEmpty(linkResult.Links);

            foreach (var link in linkResult.Links)
            {
                var linkResponse = await client.SendAsync(
                    new HttpRequestMessage(
                        HttpMethod.Options,
                        link.Url));
                linkResponse.EnsureSuccessStatusCode();
                var linkResultOfLink = await linkResponse.Content.ReadFromJsonAsync<ClientLinkResult>();
                Assert.NotNull(linkResultOfLink);
                Assert.NotEmpty(linkResultOfLink.Links);
                Assert.Contains(
                    linkResultOfLink.Links,
                    l => l.Url == link.Url && l.Urn == link.Urn);
            }
        }

        [Fact]
        public async Task OptionsLinksShouldBeLinksToOptionsForAnOptionsController()
        {
            var client = TestFactory.GetClient().AddApiKey();

            var response = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    "/api/Options"));

            response.EnsureSuccessStatusCode();
            var linkResult = await response.Content.ReadFromJsonAsync<ClientLinkResult>();
            Assert.NotNull(linkResult);
            Assert.NotEmpty(linkResult.Links);

            Assert.DoesNotContain(
                linkResult.Links,
                link => string.IsNullOrWhiteSpace(link.Url) || link.Urn != "urn:Options");
        }

        [Fact]
        public async Task OptionsShouldBeAccessibleAndReturnASelfLink()
        {
            var client = TestFactory.GetClient().AddApiKey();

            var response = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    "/api/Options"));

            response.EnsureSuccessStatusCode();
            var linkResult = await response.Content.ReadFromJsonAsync<ClientLinkResult>();
            Assert.NotNull(linkResult);
            Assert.NotEmpty(linkResult.Links);
            Assert.Contains(
                linkResult.Links,
                link => link is {Url: "/api/Options/", Urn: "urn:Options"});
        }

        [Fact]
        public async Task OptionsShouldBeAccessibleForDifferentRoles()
        {
            foreach (var role in Enum.GetValues<Role>().Select(r => r))
            {
                var client = TestFactory.GetClient().AddApiKey().AddAccessorToken(role);

                var response = await client.SendAsync(
                    new HttpRequestMessage(
                        HttpMethod.Options,
                        "/api/Options"));

                response.EnsureSuccessStatusCode();
                var linkResult = await response.Content.ReadFromJsonAsync<ClientLinkResult>();
                Assert.NotNull(linkResult);
                Assert.NotEmpty(linkResult.Links);
                Assert.Contains(
                    linkResult.Links,
                    link => link is {Url: "/api/Options/", Urn: "urn:Options"});

                foreach (var link in linkResult.Links)
                {
                    var linkResponse = await client.SendAsync(
                        new HttpRequestMessage(
                            HttpMethod.Options,
                            link.Url));
                    linkResponse.EnsureSuccessStatusCode();
                    var linkResponseResult = await linkResponse.Content.ReadFromJsonAsync<ClientLinkResult>();
                    Assert.NotNull(linkResponseResult);
                    Assert.NotEmpty(linkResponseResult.Links);
                    Assert.Contains(
                        linkResponseResult.Links,
                        l => l.Urn == "urn:Options");
                }
            }
        }
    }
}
