namespace Generic.Base.Api.Jwt
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     A service for creating json web tokens.
    /// </summary>
    internal class JwtTokenService : IJwtTokenService
    {
        /// <summary>
        ///     The jwt configuration.
        /// </summary>
        private readonly IJwtConfiguration configuration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JwtTokenService" /> class.
        /// </summary>
        /// <param name="configuration">The jwt configuration.</param>
        public JwtTokenService(IJwtConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        ///     Creates the json web token.
        /// </summary>
        /// <param name="claims">The claims of the user.</param>
        /// <returns>The token as a string.</returns>
        public string CreateToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.Key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials,
                audience: this.configuration.Audience,
                issuer: this.configuration.Issuer);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
