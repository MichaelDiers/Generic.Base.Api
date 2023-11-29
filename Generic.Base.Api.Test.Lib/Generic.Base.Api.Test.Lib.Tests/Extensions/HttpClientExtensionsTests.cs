namespace Generic.Base.Api.Test.Lib.Tests.Extensions
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Test.Lib.CrudTest;
    using Generic.Base.Api.Test.Lib.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    ///     Tests for <see cref="HttpClientExtensions" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
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
                client.DefaultRequestHeaders.GetValues(HttpClientExtensions.XApiKeyName).FirstOrDefault());
        }

        [Fact]
        public void AddToken()
        {
            const string token = nameof(token);

            using var client = new HttpClient();
            client.AddToken(token);

            Assert.Equal(
                token,
                client.DefaultRequestHeaders.Authorization?.Parameter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("userid")]
        [InlineData(
            null,
            Role.Admin)]
        [InlineData(
            "userId",
            Role.Admin,
            Role.User)]
        public void AddTokenClaims(string? userId, params Role[] roles)
        {
            HttpClientExtensionsTests.SetUp();

            var claims = roles.Select(
                role => new Claim(
                    ClaimTypes.Role,
                    role.ToString()));
            if (userId != null)
            {
                claims = claims.Append(
                    new Claim(
                        Constants.UserIdClaimType,
                        userId));
            }

            using var client = new HttpClient();
            client.AddToken(claims.ToArray());

            var token = client.DefaultRequestHeaders.Authorization?.Parameter;
            Assert.NotNull(token);

            var decoded = ClientJwtTokenService.Decode(token);
            foreach (var role in roles)
            {
                Assert.Contains(
                    decoded.Claims,
                    claim => claim.Type == ClaimTypes.Role && claim.Value == role.ToString());
            }

            if (userId != null)
            {
                Assert.Contains(
                    decoded.Claims,
                    claim => claim.Type == Constants.UserIdClaimType && claim.Value == userId);
            }
            else
            {
                Assert.DoesNotContain(
                    decoded.Claims,
                    claim => claim.Type == Constants.UserIdClaimType && claim.Value == userId);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("userid")]
        [InlineData(
            null,
            Role.Admin)]
        [InlineData(
            "userId",
            Role.Admin,
            Role.User)]
        public void AddTokenRoles(string? userId, params Role[] roles)
        {
            HttpClientExtensionsTests.SetUp();

            using var client = new HttpClient();
            client.AddToken(
                roles,
                userId);

            var token = client.DefaultRequestHeaders.Authorization?.Parameter;
            Assert.NotNull(token);

            var decoded = ClientJwtTokenService.Decode(token);
            foreach (var role in roles)
            {
                Assert.Contains(
                    decoded.Claims,
                    claim => claim.Type == ClaimTypes.Role && claim.Value == role.ToString());
            }

            if (userId != null)
            {
                Assert.Contains(
                    decoded.Claims,
                    claim => claim.Type == Constants.UserIdClaimType && claim.Value == userId);
            }
            else
            {
                Assert.DoesNotContain(
                    decoded.Claims,
                    claim => claim.Type == Constants.UserIdClaimType && claim.Value == userId);
            }
        }

        [Theory]
        [InlineData(
            null,
            null)]
        [InlineData(
            "apiKey",
            null)]
        [InlineData(
            null,
            "token")]
        [InlineData(
            "apiKey",
            "token")]
        public void Clear(string? apiKey, string? token)
        {
            using var client = new HttpClient();
            if (apiKey is not null)
            {
                client.AddApiKey(apiKey);
            }

            if (token is not null)
            {
                client.AddToken(token);
            }

            client.Clear();

            Assert.Null(client.DefaultRequestHeaders.Authorization);
            Assert.False(
                client.DefaultRequestHeaders.TryGetValues(
                    HttpClientExtensions.XApiKeyName,
                    out _));
        }

        private static void SetUp()
        {
            var configuration = new HostApplicationBuilder().Configuration
                .GetSection(JwtConfiguration.ConfigurationSection)
                .Get<JwtConfiguration>();
            Assert.NotNull(configuration);
            Environment.SetEnvironmentVariable(
                configuration.KeyName,
                "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx");
        }
    }
}
