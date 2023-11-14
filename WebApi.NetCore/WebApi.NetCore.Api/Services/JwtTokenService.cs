namespace WebApi.NetCore.Api.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using WebApi.NetCore.Api.Contracts;
    using WebApi.NetCore.Api.Contracts.Configuration;
    using WebApi.NetCore.Api.Contracts.Services;

    /// <summary>
    ///     A service for creating json web tokens.
    /// </summary>
    internal class JwtTokenService : IJwtTokenService
    {
        /// <summary>
        ///     The environment configuration.
        /// </summary>
        private readonly IEnvConfiguration envConfiguration;

        /// <summary>
        ///     The jwt configuration.
        /// </summary>
        private readonly IJwtConfiguration jwtConfiguration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JwtTokenService" /> class.
        /// </summary>
        /// <param name="jwtConfiguration">The jwt configuration.</param>
        /// <param name="envConfiguration">The environment configuration.</param>
        public JwtTokenService(IJwtConfiguration jwtConfiguration, IEnvConfiguration envConfiguration)
        {
            this.jwtConfiguration = jwtConfiguration;
            this.envConfiguration = envConfiguration;
        }

        /// <summary>
        ///     Creates the token.
        /// </summary>
        /// <param name="roles">The roles of the user.</param>
        /// <returns>The token as a string.</returns>
        public string CreateToken(IEnumerable<Role> roles)
        {
            var claims = roles.Select(
                    role => new Claim(
                        ClaimTypes.Role,
                        role.ToString()))
                .Append(
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        Guid.NewGuid().ToString()))
                .ToArray();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.envConfiguration.JwtKey));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials,
                audience: this.jwtConfiguration.Audience,
                issuer: this.jwtConfiguration.Issuer);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
