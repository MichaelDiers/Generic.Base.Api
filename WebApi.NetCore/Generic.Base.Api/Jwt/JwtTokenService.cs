namespace Generic.Base.Api.Jwt
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Generic.Base.Api.AuthService.UserService;
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
        /// <param name="id">The identifier of the user.</param>
        /// <param name="claims">The claims of the user.</param>
        /// <returns>An <see cref="IToken" /> that contains access and refresh token.</returns>
        public IToken CreateToken(string id, IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.Key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);

            var accessToken = this.CreateToken(
                claims.Concat(
                    new[]
                    {
                        new Claim(
                            ClaimTypes.Role,
                            nameof(Role.Accessor)),
                        new Claim(
                            ClaimTypes.Name,
                            id)
                    }),
                this.configuration.AccessTokenExpires,
                signingCredentials);

            var refreshToken = this.CreateToken(
                new[]
                {
                    new Claim(
                        ClaimTypes.Role,
                        nameof(Role.Refresher)),
                    new Claim(
                        ClaimTypes.Name,
                        id)
                },
                this.configuration.RefreshTokenExpires,
                signingCredentials);

            return new Token(
                accessToken,
                refreshToken);
        }

        /// <summary>
        ///     Creates the token.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="expiresIn">The expires in minutes specification.</param>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <returns>A new token.</returns>
        private string CreateToken(IEnumerable<Claim> claims, int expiresIn, SigningCredentials signingCredentials)
        {
            var accessToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresIn),
                signingCredentials: signingCredentials,
                audience: this.configuration.Audience,
                issuer: this.configuration.Issuer);

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}
