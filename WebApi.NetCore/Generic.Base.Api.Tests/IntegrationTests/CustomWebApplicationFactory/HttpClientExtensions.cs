namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Tests.Lib;

    /// <summary>
    ///     Extensions for <see cref="HttpClient" />.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        ///     Adds the accessor token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>The given <see cref="HttpClient" />.</returns>
        public static HttpClient AddAccessorToken(this HttpClient client, params Role[] roles)
        {
            return client.AddToken(
                true,
                roles);
        }

        /// <summary>
        ///     Adds the API key.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>The given <see cref="HttpClient" />.</returns>
        public static HttpClient AddApiKey(this HttpClient client)
        {
            client.DefaultRequestHeaders.Add(
                "x-api-key",
                TestFactory.ApiKey);
            return client;
        }

        /// <summary>
        ///     Adds the refresher token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>The given <see cref="HttpClient" />.</returns>
        public static HttpClient AddRefresherToken(this HttpClient client, params Role[] roles)
        {
            return client.AddToken(
                false,
                roles);
        }

        /// <summary>
        ///     Adds the token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="accessor">if set to <c>true</c> [accessor].</param>
        /// <param name="roles">The roles.</param>
        /// <returns>The given <see cref="HttpClient" />.</returns>
        private static HttpClient AddToken(this HttpClient client, bool accessor, IEnumerable<Role> roles)
        {
            var jwtService =
                TestHostApplicationBuilder.GetService<IJwtTokenService>(JwtTokenServiceDependencies.AddJwtTokenService);
            var claims = roles.Select(
                role => new Claim(
                    ClaimTypes.Role,
                    role.ToString()));

            var id = TestFactory.DefaultId;
            var displayName = TestFactory.DefaultDisplayName;

            var token = client.DefaultRequestHeaders.Authorization?.Parameter?.Replace(
                "Bearer",
                string.Empty);
            if (token is not null)
            {
                var decoded = jwtService.Decode(token);
                claims = claims.Concat(decoded.Claims).ToArray();
                id = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                     TestFactory.DefaultId;
                displayName = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value ??
                              TestFactory.DefaultId;
            }

            var tokens = jwtService.CreateToken(
                id,
                displayName,
                claims);

            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {tokens.AccessToken}");

            return client;
        }
    }
}
