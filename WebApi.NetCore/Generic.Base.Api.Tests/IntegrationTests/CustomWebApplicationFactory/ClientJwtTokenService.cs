namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Jwt;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     Create jwt for testing.
    /// </summary>
    internal static class ClientJwtTokenService
    {
        /// <summary>
        ///     Creates a token.
        /// </summary>
        /// <param name="roles">The roles that are added as claims.</param>
        /// <returns>The generated token.</returns>
        public static string CreateToken(params Role[] roles)
        {
            var configuration = new HostApplicationBuilder().Configuration.GetSection("Jwt").Get<JwtConfiguration>();
            Assert.NotNull(configuration);

            var key = Environment.GetEnvironmentVariable(configuration.KeyName);
            Assert.NotNull(key);
            configuration.Key = key;

            return ClientJwtTokenService.CreateToken(
                roles.Select(
                    role => new Claim(
                        ClaimTypes.Role,
                        role.ToString())),
                DateTime.UtcNow.AddMinutes(configuration.AccessTokenExpires),
                DateTime.UtcNow,
                configuration.Issuer,
                configuration.Audience,
                configuration.Key);
        }

        /// <summary>
        ///     Creates a new token.
        /// </summary>
        /// <param name="claims">The claims that are added to the token.</param>
        /// <param name="expires">Specifies when the token expires.</param>
        /// <param name="notBefore">Specifies from when the token is valid.</param>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience of the token.</param>
        /// <param name="key">The key for generating the token.</param>
        /// <returns>The generated token.</returns>
        public static string CreateToken(
            IEnumerable<Claim> claims,
            DateTime expires,
            DateTime notBefore,
            string issuer,
            string audience,
            string key
        )
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials,
                audience: audience,
                issuer: issuer,
                notBefore: notBefore);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        ///     Decodes the specified token without validation.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The decoded token.</returns>
        public static JwtSecurityToken Decode(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
    }
}
